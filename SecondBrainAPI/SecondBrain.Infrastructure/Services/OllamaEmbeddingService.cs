using SecondBrain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SecondBrain.Infrastructure.Services
{
    public class OllamaEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient http;
        private const string Model = "nomic-embed-text";

        public OllamaEmbeddingService(HttpClient http)
        {
            this.http = http;
            this.http.BaseAddress = new Uri("http://localhost:11434");
            this.http.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<float[]> EmbedAsync(string text)
        {
            var results = await EmbedBatchAsync([text]);
            return results[0];
        }

        public async Task<List<float[]>> EmbedBatchAsync(List<string> texts)
        {
            var results = new float[texts.Count][];

            await Parallel.ForEachAsync(
                texts.Select((text, i) => (text, i)), 
                new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                async (item, ct) =>
                {
                    results[item.i] = await EmbedSingleAsync(item.text);
                });

            return results.ToList();
        }

        private async Task<float[]> EmbedSingleAsync(string text)
        {
            var response = await this.http.PostAsJsonAsync("/api/embeddings", new
            {
                model = Model,
                prompt = text
            });

            var raw = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OllamaEmbeddingResponse>(raw);

            return result?.Embedding ?? [];
        }
    }

    public record OllamaEmbeddingResponse([property: JsonPropertyName("embedding")] float[] Embedding);
}
