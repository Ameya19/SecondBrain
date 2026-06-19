using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondBrain.Core.Entities;
using SecondBrain.Infrastructure.Data;

namespace SecondBrain.API.Controllers
{
    [ApiController]
    [Route("api/sources")]
    public class SourcesController : ControllerBase
    {
        private readonly SecondBrainDbContext dbContext;

        public SourcesController(SecondBrainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSources()
        {
            var sources = await this.dbContext.Sources
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Type,
                    s.Url,
                    s.IngestedAt,
                    s.PublishedAt,
                    s.Tags,
                    ChunkCount = s.KnowledgeChunks.Count
                })
                .OrderByDescending(s => s.IngestedAt)
                .ToListAsync();

            if (!sources.Any())
            {
                return NotFound();
            }
            return Ok(sources);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSourceById([FromRoute]Guid id)
        {
            var source = await this.dbContext.Sources
                .Where(i => i.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Url,
                    s.Type,
                    s.IngestedAt,
                    s.PublishedAt,
                    s.Tags,
                    ChunkCount = s.KnowledgeChunks.Count,
                    IngestionJob = s.IngestionJobs
                        .OrderByDescending(j => j.StartedAt)
                        .Select(t => new
                        {
                            t.Status,
                            t.StartedAt,
                            t.CompletedAt,
                            t.ChunksCreated,
                            t.ErrorMessage
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (source is null)
            {
                return NotFound(new { Message = $"Source {id} not found" });
            }
            return Ok(source);
        }

        [HttpGet("{id}/chunks")]
        public async Task<IActionResult> GetChunksById([FromRoute]Guid id)
        {
            var sourceExists = await this.dbContext.Sources.AnyAsync(s => s.Id == id);

            if(!sourceExists)
            {
                return NotFound();
            }

            var chunks = await this.dbContext.KnowledgeChunks
                .Where(c => c.SourceId == id)
                .OrderBy(c => c.IngestedAt)
                .Select(c => new
                {
                    c.Id,
                    c.IngestedAt,
                    c.AccessCount,
                    c.LastAccessedAt,
                    Preview = c.Content.Substring(0, Math.Min(100, c.Content.Length))
                }).ToListAsync();

            return Ok(new
            {
                SourceId = id,
                TotalChunks = chunks.Count,
                Chunks = chunks
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSourceById([FromRoute] Guid id)
        {
            var source = await this.dbContext.Sources.FindAsync(id);

            if (source == null)
            {
                return NotFound();
            }

            dbContext.Sources.Remove(source);
            await this.dbContext.SaveChangesAsync();

            return Ok($"Source with {id} has been deleted successfully.");
        }

        [HttpPatch("{id}/patch")]
        public async Task<IActionResult> UpdateTagsBySourceId([FromRoute] Guid id, [FromBody] string[] tags)
        {
            var source = await this.dbContext.Sources.FindAsync(id);

            if (source == null)
            {
                return NotFound($"Source with {id} not found.");
            }

            source.Tags = tags;
            await this.dbContext.SaveChangesAsync();

            return Ok(new
            {
                SourceId = id,
                Tags = tags
            });
        }
    }
}
