using SecondBrain.Core.Interfaces;
using SecondBrain.Core.Models;
using SecondBrain.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Pgvector;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using SecondBrain.Core.Entities;

namespace SecondBrain.Infrastructure.Services
{
    public class QueryService : IQueryService
    {
        private readonly SecondBrainDbContext dbContext;
        private readonly IEmbeddingService embeddingService;
        private readonly IGenerationService generationService;

        public QueryService(SecondBrainDbContext dbContext, IEmbeddingService embeddingService, IGenerationService generationService)
        {
            this.dbContext = dbContext;
            this.embeddingService = embeddingService;
            this.generationService = generationService;
        }

        public async Task<QueryResult> QueryAsync(string question, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            var now = DateTimeOffset.UtcNow;

            // Embed the question
            var queryEmbedding = await embeddingService.EmbedAsync(question);
            var vector = new Pgvector.Vector(queryEmbedding);

            // Retrieve the top 20 candidates using cosine similarity
            var candidates = await dbContext.KnowledgeChunks
                .Include(c => c.Source)
                .OrderBy(c => c.Embedding.CosineDistance(vector))
                .Take(20)
                .AsNoTracking()
                .ToListAsync();

            if (!candidates.Any())
            {
                return new QueryResult("No relevant knowledge found. Try ingesting some content first.", [], []);
            }

            //Temporal Reranking
            var reranked = options.UseTemporalReranking ? TemporalRerank(candidates, vector, now, options.TopK) : candidates.Take(options.TopK).ToList();

            //Detect Contradiction
            var contradiction = DetectContradiction(reranked);

            //Build prompt with Temporal Metadata
            var contextBlocks = reranked.Select(c =>
                $"[Source: {c.Source.Title} | " +
                $"Added: {c.IngestedAt:yyyy-MM-dd} | " +
                $"Published: {c.Source.PublishedAt?.ToString("yyyy-MM-dd") ?? "unknown"}]\n" + 
                c.Content
            );

            var context = string.Join("\n\n---\n\n", contextBlocks);

            var contradictionBlock = contradiction.Any() ? $"\nPotential contradictions detected:\n{string.Join("\n", contradiction)}\n" : "";

            var prompt = $"""
                <start_of_turn>user
                You are a personal knwoledge assistant. Answer me the question using only the provided knowledge chunks. Each chunks has a source title and date.

                Rules:
                - If two chunks contradict each other, prefer the more recent one but explicitly note the contradiction and both dates.
                - If knowledge is older than 2 years, flag it as potentially outdated.
                - Always cite source with their dates at the end of the answer.
                - If the context does not contain enough information, say it clearly.

                Knowledge:
                {context}
                {contradictionBlock}
                Question: {question}
                <end_of_turn>
                <start_of_turn>model
                """;

            var answer = await generationService.GenerateAsync(prompt);

            await PersistQueryAsync(question, answer, reranked, now);

            return new QueryResult(answer, reranked.Select(c => new SourceRef(
                c.Source.Title,
                c.IngestedAt,
                c.Source.PublishedAt
                )).ToList(),
                contradiction
            );
        }

        public List<KnowledgeChunk> TemporalRerank(List<KnowledgeChunk> candidates, Pgvector.Vector vector, DateTimeOffset now, int TopK)
        {
            return candidates
                .Select(c =>
                {
                    double similarity = 1 - (double)c.Embedding.CosineDistance(vector);

                    //Recency Score: exponential decay over 365 days
                    double daysSinceIngested = (now - c.IngestedAt).TotalDays;
                    double recencyScore = Math.Exp(-daysSinceIngested / 365);

                    //Access score: frequently asked chunks bubble up
                    double accessScore = Math.Log(c.AccessCount + 1) / 10.0;

                    double finalScore = (similarity * 0.6) + (recencyScore * 0.25) + (accessScore * 0.15);

                    return (Chunk: c, Score: finalScore);
                })
                .OrderByDescending(x => x.Score)
                .Take(TopK)
                .Select(x => x.Chunk)
                .ToList();
        }

        public List<string> DetectContradiction(List<KnowledgeChunk> chunks)
        {
            var contraditions = new List<string>();

            for (int i = 0; i < chunks.Count; i++)
            {
                for (int j = i + 1; j < chunks.Count; j++)
                {
                    var a = chunks[i];
                    var b = chunks[j];

                    if (a.Source.Url != null && b.Source.Url != null)
                    {
                        try
                        {
                            var hostA = new Uri(a.Source.Url).Host;
                            var hostB = new Uri(b.Source.Url).Host;

                            var gap = Math.Abs((a.IngestedAt - b.IngestedAt).TotalDays);

                            if(hostA == hostB && gap > 180)
                            {
                                contraditions.Add(
                                    $"'{a.Source.Title}' ({a.IngestedAt:yyyy-MM-dd}) vs " +
                                    $"'{b.Source.Title}' ({b.IngestedAt:yyyy-MM-dd}) " +
                                    $"— same source, {gap:0} days apart"
                                );
                            }
                        }
                        catch (UriFormatException)
                        {
                        }
                    }
                }
            }

            return contraditions;
        }

        private async Task PersistQueryAsync(string question, string answer, List<KnowledgeChunk> usedChunks, DateTimeOffset now)
        {
            var query = new Core.Entities.Query
            {
                Id = new Guid(),
                Question = question,
                Answer = answer,
                AskedAt = now,
                ModelUsed = "gemma4:latest"
            };

            dbContext.Queries.Add(query);

            // Embed the question once — reuse for all relevance scores
            var questionEmbedding = await embeddingService.EmbedAsync(question);
            var questionVector = new Pgvector.Vector(questionEmbedding);

            var links = usedChunks.Select(c => new QueryChunkLink
            {
                QueryId = query.Id,
                ChunkId = c.Id,

                RelevanceScore = (float)c.Embedding.CosineDistance(questionVector)
            });

            dbContext.QueryChunkLinks.AddRange(links);

            //Update access metadata on chunks (tracked entities)
            var trackedChunks = await dbContext.KnowledgeChunks.Where(c => usedChunks.Select(uc => uc.Id).Contains(c.Id)).ToListAsync();

            foreach (var chunk in trackedChunks)
            {
                chunk.LastAccessedAt = now;
                chunk.AccessCount++;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
