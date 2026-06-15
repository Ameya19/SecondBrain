using SecondBrain.Core.Entities;
using SecondBrain.Core.Interfaces;
using SecondBrain.Core.Models;
using SecondBrain.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Infrastructure.Services
{
    public class IngestionService : IIngestionService
    {
        private readonly SecondBrainDbContext dbContext;
        private readonly IEmbeddingService embeddingService;

        public IngestionService(SecondBrainDbContext dbContext, IEmbeddingService embeddingService)
        {
            this.dbContext = dbContext;
            this.embeddingService = embeddingService;
        }

        public async Task<Guid> IngestAsync(string content, SourceMetadata metadata)
        {
            //Create and save the source
            var source = new Source
            {
                Id = Guid.NewGuid(),
                Title = metadata.Title,
                Type = metadata.Type,
                Url = metadata.Url,
                PublishedAt = metadata.PublishedAt,
                IngestedAt = DateTimeOffset.UtcNow,
                Tags = metadata.Tags
            };

            dbContext.Sources.Add(source);

            //Create ingestion job to track progress
            var ingestionJob = new IngestionJob
            {
                Id = Guid.NewGuid(),
                SourceId = source.Id,
                Status = "Processing",
                StartedAt = DateTimeOffset.UtcNow
            };

            dbContext.IngestionJobs.Add(ingestionJob);
            await dbContext.SaveChangesAsync();

            try
            {
                //Chunk the content
                var chunks = ChunkText(content, maxChunkSize: 1600, overlap: 200);
                Console.WriteLine($"Chunked into {chunks.Count} chunks");

                //Embed all chunks
                var embeddings = await embeddingService.EmbedBatchAsync(chunks);

                //Save Chunks to Database
                var knowledgeChunks = chunks.Zip(embeddings).Select(pair => new KnowledgeChunk
                {
                    Id = Guid.NewGuid(),
                    SourceId = source.Id,
                    Content = pair.First,
                    IngestedAt = DateTimeOffset.Now,
                    AccessCount = 0,
                    Embedding = new Pgvector.Vector(pair.Second)
                }).ToList();

                dbContext.KnowledgeChunks.AddRange(knowledgeChunks);

                //Update Job Status
                ingestionJob.Status = "Done";
                ingestionJob.CompletedAt = DateTimeOffset.UtcNow;
                ingestionJob.ChunksCreated = knowledgeChunks.Count;

                await dbContext.SaveChangesAsync();

                Console.WriteLine($">>> Ingestion complete. Source: {source.Id}, Chunks: {knowledgeChunks.Count}");

                return source.Id;
            }
            catch(Exception ex)
            {
                ingestionJob.Status = "Failed";
                ingestionJob.CompletedAt = DateTimeOffset.UtcNow;
                ingestionJob.ErrorMessage = ex.Message;
                await dbContext.SaveChangesAsync();

                Console.WriteLine($">>> Ingestion failed: {ex.Message}");
                throw;
            }
        }

        private List<string> ChunkText(string text, int maxChunkSize, int overlap)
        {
            var chunks = new List<string>();

            var paragraphs = text
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .ToList();

            var current = new System.Text.StringBuilder();

            foreach (var paragraph in paragraphs)
            {
                if (current.Length + paragraph.Length > maxChunkSize && current.Length > 0)
                {
                    chunks.Add(current.ToString().Trim());

                    var currentText = current.ToString();
                    current.Clear();

                    if(currentText.Length > overlap)
                    {
                        current.Append(currentText[^overlap..]);
                    }
                    else
                    {
                        current.Append(currentText);
                    }
                }

                current.AppendLine(paragraph);
            }

            if (current.Length > 0)
            {
                chunks.Add(current.ToString().Trim());
            }

            return chunks;
        }
    }
}
