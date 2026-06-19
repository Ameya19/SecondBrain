using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondBrain.Infrastructure.Data;

namespace SecondBrain.API.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly SecondBrainDbContext dbContext;

        public SearchController(SecondBrainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET /api/search?q=rag&from=2026-01-01&to=2026-06-01&type=note&tag=ai
        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string? q,
            [FromQuery] string? from,
            [FromQuery] string? to,
            [FromQuery] string? type,
            [FromQuery] string? tag,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                page = 1;

            if (pageSize > 50)
                pageSize = 50;

            var query = dbContext.KnowledgeChunks.Include(c => c.Source).AsNoTracking().AsQueryable();

            //Keyword filter - Search in content and source title
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(c => c.Content.ToLower().Contains(q.ToLower()) || c.Source.Title.ToLower().Contains(q.ToLower()));
            }

            //Date range filter on ingestion date
            if (DateTimeOffset.TryParse(from, out var fromDate))
            {
                query = query.Where(c => c.IngestedAt >= fromDate.ToUniversalTime());
            }

            if (DateTimeOffset.TryParse(to, out var toDate))
            {
                query = query.Where(c => c.IngestedAt <= toDate.ToUniversalTime());
            }

            //Source type filter
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(c => c.Source.Type == type);
            }

            //Tag Filter
            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(c => c.Source.Tags != null && c.Source.Tags.Contains(tag));
            }

            var totalCount = await query.CountAsync();

            var results = await query.OrderByDescending(c => c.IngestedAt).Skip((page - 1) * pageSize).Take(pageSize).Select(c => new
            {
                c.Id,
                c.IngestedAt,
                c.AccessCount,
                Preview = c.Content.Substring(0, Math.Min(150, c.Content.Length)),
                Source = new {
                    c.Source.Id,
                    c.Source.Title,
                    c.Source.Type,
                    c.Source.Url,
                    c.Source.Tags
                }
            }).ToListAsync();

            return Ok(new
            {
                Query = q,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Results = results
            });
        }

        // GET /api/search/sources?q=rag&type=pdf
        [HttpGet("sources")]
        public async Task<IActionResult> SearchSources(
            [FromQuery]string? q,
            [FromQuery]string? type,
            [FromQuery]string? tag)
        {
            var query = dbContext.Sources.AsNoTracking().AsQueryable();

            if(!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c => c.Title.ToLower().Contains(q.ToLower()));
            }

            if(!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(c => c.Type == type);
            }

            if(!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(c => c.Tags != null && c.Tags.Contains(tag));
            }

            var results = await query.OrderByDescending(c => c.IngestedAt)
                .Select(q => new
                {
                    q.Id,
                    q.Title,
                    q.Type,
                    q.Url,
                    q.IngestedAt,
                    q.Tags,
                    ChunkCount = q.KnowledgeChunks.Count
                }).ToListAsync();

            return Ok(results);
        }

        // GET /api/search/timeline?tag=ai
        [HttpGet("timeline")]
        public async Task<IActionResult> Timeline([FromQuery]string? tag)
        {
            var query = dbContext.Sources.AsNoTracking().AsQueryable();

            if(!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(c => c.Tags != null && c.Tags.Contains(tag));
            }

            // Pull raw data from DB — no date functions in the SQL translation
            var sources = await query
                .Select(s => new { s.IngestedAt, s.Type, s.Title })
                .ToListAsync();

            // Group in memory using plain C# — works regardless of provider quirks
            var timeline = sources
                .GroupBy(s => new { s.IngestedAt.Year, s.IngestedAt.Month })
                .Select(g => new
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count(),
                    Types = g.GroupBy(s => s.Type)
                             .Select(t => new { Type = t.Key, Count = t.Count() })
                             .ToList(),
                    Sources = g.Select(s => s.Title).Take(3).ToList()
                })
                .OrderByDescending(x => x.Period)
                .ToList();

            return Ok(timeline);
        }
    }
}
