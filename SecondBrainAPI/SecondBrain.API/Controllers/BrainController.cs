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
        private readonly IGenerationService generationService;
        private readonly IQueryService queryService;

        public BrainController(IEmbeddingService embeddingService, IIngestionService ingestionService, IGenerationService generationService, IQueryService queryService)
        {
            this.embeddingService = embeddingService;
            this.ingestionService = ingestionService;
            this.generationService = generationService;
            this.queryService = queryService;
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

        [HttpPost("test-generate")]
        public async Task<IActionResult> TestGenerate([FromBody] string prompt)
        {
            var response = await generationService.GenerateAsync(prompt);
            return Ok(new
            {
                Response = response
            });
        }

        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] QueryRequest request)
        {
            var result = await this.queryService.QueryAsync(request.Question, new QueryOptions(request.TopK ?? 5));
            return Ok(result);
        }
    }
}
