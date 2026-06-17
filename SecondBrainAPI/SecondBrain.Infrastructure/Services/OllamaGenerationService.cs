using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SecondBrain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecondBrain.Infrastructure.Services
{
    public class OllamaGenerationService : IGenerationService
    {
        private readonly HttpClient http;
        private const string Model = "gemma4:latest";

        public OllamaGenerationService(HttpClient http)
        {
            this.http = http;
            this.http.BaseAddress = new Uri("http://localhost:11434");
            this.http.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var response = await this.http.PostAsJsonAsync("/api/generate", new
            {
                prompt,
                model = Model,
                stream = false
            });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>();

            return result?.Response ?? throw new InvalidOperationException("Ollama returned null response");
        }

        public async IAsyncEnumerable<string> StreamAsync(string prompt)
        {
            var ct = CancellationToken.None;

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/generate")
            {
                Content = JsonContent.Create(new
                {
                    model = Model,
                    prompt,
                    stream = true
                })
            };

            using var response = await this.http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);

            using var reader = new StreamReader(stream);

            while(!reader.EndOfStream && !ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(ct);
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var chunk = JsonSerializer.Deserialize<OllamaStreamChunk>(line);

                if (chunk?.Done == true)
                    yield break;

                if(chunk?.Response != null)
                    yield return chunk.Response;
            }
        }
    }

    public record OllamaGenerateResponse([property: JsonPropertyName("response")] string Response);

    public record OllamaStreamChunk(
        [property: JsonPropertyName("done")] bool Done,
        [property: JsonPropertyName("response")] string Response
    );
}
