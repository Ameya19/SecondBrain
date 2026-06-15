using Microsoft.AspNetCore.Mvc;
using SecondBrain.Core.Interfaces;
using SecondBrain.Core.Models;

namespace SecondBrain.API.Controllers
{
    [ApiController]
    [Route("api/brain")]
    public class BrainController : ControllerBase
    {
        private readonly IEmbeddingService embeddingService;
        private readonly IIngestionService ingestionService;

        public BrainController(IEmbeddingService embeddingService, IIngestionService ingestionService)
        {
            this.embeddingService = embeddingService;
            this.ingestionService = ingestionService;
        }

        [HttpPost("test-embed")]
        public async Task<IActionResult> TestEmbed([FromBody] string text)
        {
            Console.WriteLine($">>> TestEmbed hit with: {text}");
            var embedding = await this.embeddingService.EmbedAsync(text);

            return Ok(new
            {
                Dimentions = embedding.Length,
                Sample = embedding.Take(5)
            });
        }

        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest([FromBody]IngestRequest request)
        {
            var sourceId = await ingestionService.IngestAsync(request.Content, new SourceMetadata(request.Title, request.Type, request.Url, request.PublishedAt, request.Tags));

            return Ok(new { SourceId = sourceId });
        }
    }
}
