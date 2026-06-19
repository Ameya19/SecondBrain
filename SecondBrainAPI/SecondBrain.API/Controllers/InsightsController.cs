using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondBrain.Infrastructure.Data;

namespace SecondBrain.API.Controllers
{
    [ApiController]
    [Route("api/insights")]
    public class InsightsController : ControllerBase
    {
        private readonly SecondBrainDbContext dbContext;

        public InsightsController(SecondBrainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("contradictions")]
        public async Task<IActionResult> GetContradictions()
        {
            var contraditions = await dbContext.Contradictions
                .Include(c => c.ChunkA)
                .ThenInclude(s => s.Source)
                .Include(c => c.ChunkB)
                .ThenInclude(s => s.Source)
                .Where(c => !c.Resolved)
                .OrderByDescending(c => c.DetectedAt)
                .Select(c => new
                {
                    c.Id,
                    ChunkA = new
                    {
                        c.ChunkA.Id,
                        c.ChunkA.IngestedAt,
                        Source = c.ChunkA.Source.Title,
                        Preview = c.ChunkA.Content.Substring(0, Math.Min(100, c.ChunkA.Content.Length))
                    },
                    ChunkB = new
                    {
                        c.ChunkB.Id,
                        c.ChunkB.IngestedAt,
                        Source = c.ChunkB.Source.Title,
                        Preview = c.ChunkB.Content.Substring(0, Math.Min(100, c.ChunkB.Content.Length))
                    },
                    DaysBetween = Math.Abs((c.ChunkA.IngestedAt - c.ChunkB.IngestedAt).TotalDays),
                    c.DetectedAt,
                    c.Resolved,
                    c.ResolutionNote
                })
                .ToListAsync();

            return Ok(new
            {
                TotalContradictions = contraditions.Count,
                Unresolved = contraditions.Count(c => !c.Resolved),
                Items = contraditions
            });
        }

        [HttpPatch("contradictions/{id}/resolve")]
        public async Task<IActionResult> ResolveContradiction([FromRoute] Guid id, [FromBody] string? note)
        {
            var contradiction = await dbContext.Contradictions.FindAsync(id);
            if (contradiction == null)
            {
                return NotFound(new { Message = $"Contradiction {id} not found." });
            }

            contradiction.Resolved = true;
            contradiction.ResolutionNote = note;
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "Contradition marked as resolved", contradiction.Id });
        }

        // GET /api/insights/decay?days=365
        [HttpGet("decay")]
        public async Task<IActionResult> GetKnowledgeDecay([FromQuery]int days = 365)
        {
            var cutoffDate = DateTimeOffset.UtcNow.AddDays(-days);

            var decayedChunks = await dbContext.KnowledgeChunks
                .Include(c => c.Source)
                .Where(c => c.IngestedAt < cutoffDate && (c.LastAccessedAt == null || c.LastAccessedAt < cutoffDate))
                .OrderBy(c => c.IngestedAt)
                .Select(c => new
                {
                    c.Id,
                    c.IngestedAt,
                    c.LastAccessedAt,
                    c.AccessCount,
                    Source = c.Source.Title,
                    Preview = c.Content.Substring(0, Math.Min(100, c.Content.Length)),
                    DaysSinceAdded = (int)(DateTimeOffset.UtcNow - c.IngestedAt).TotalDays,
                    DaysSinceLastAccess = c.LastAccessedAt.HasValue ? (int)(DateTimeOffset.UtcNow - c.LastAccessedAt.Value).TotalDays : (int)(DateTimeOffset.UtcNow - c.IngestedAt).TotalDays
                })
                .ToListAsync();

            return Ok(new
            {
                Threshold = $"{days} days",
                Count = decayedChunks.Count(),
                PercentageOfTotal = decayedChunks.Any() ? Math.Round((double)decayedChunks.Count / await dbContext.KnowledgeChunks.CountAsync() * 100 / 2) : 0,
                Items = decayedChunks
            });
        }

        // GET /api/insights/growth?period=month
        [HttpGet("growth")]
        public async Task<IActionResult> GetIngestionGrowth([FromQuery]string period = "month")
        {
            var sources = await dbContext.Sources.Select(c => new { c.IngestedAt, c.Type }).ToListAsync();

            object timeline = period.ToLower() switch
            {
                "week" => sources.GroupBy(s => new { s.IngestedAt.Year, Week = GetWeekNumber(s.IngestedAt) }).Select(s => new
                {
                    Period = $"{s.Key.Year} - W{s.Key.Week:D2}",
                    Count = s.Count(),
                    Types = s.GroupBy(s => s.Type).Select(t => new { Type = t.Key, Count = t.Count() })
                })
                .OrderByDescending(x => x.Period)
                .ToList(),

                "month" => sources.GroupBy(s => new { s.IngestedAt.Year, s.IngestedAt.Month }).Select(s => new
                {
                    Period = $"{s.Key.Year} - {s.Key.Month:D2}",
                    Count = s.Count(),
                    Types = s.GroupBy(s => s.Type).Select(t => new { Type = t.Key, Count = t.Count() })
                })
                .OrderByDescending(x => x.Period)
                .ToList(),

                "quarter" => sources.GroupBy(s => new { s.IngestedAt.Year, Quarter = (s.IngestedAt.Month - 1) / 3 + 1 }).Select(s => new
                {
                    Period = $"{s.Key.Year} - Q{s.Key.Quarter}",
                    Count = s.Count(),
                    Types = s.GroupBy(s => s.Type).Select(t => new { Type = t.Key, Count = t.Count() })
                })
                .OrderByDescending(x => x.Period)
                .ToList(),

                "year" => sources.GroupBy(s => s.IngestedAt.Year).Select(s => new
                {
                    Period = s.Key.ToString(),
                    Count = s.Count(),
                    Types = s.GroupBy(s => s.Type).Select(t => new { Type = t.Key, Count = t.Count() })
                })
                .OrderByDescending(x => x.Period)
                .ToList(),

                _ => new List<object>()
            };

            var timelineList = (dynamic)timeline;
            var allSets = sources.Count();
            var avgPerPeriod = allSets > 0 ? Math.Round((double)allSets / timelineList.Count, 2) : 0;

            return Ok(new
            {
                Period = period,
                TotalSources = allSets,
                AvgPerPeriod = avgPerPeriod,
                Timeline = timeline
            });
        }

        // GET /api/insights/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetOverallStats()
        {
            var totalChunks = await dbContext.KnowledgeChunks.CountAsync();
            var totalSources = await dbContext.Sources.CountAsync();
            var totalQueries = await dbContext.Queries.CountAsync();

            var accessStats = await dbContext.KnowledgeChunks.GroupBy(c => true)
                .Select(g => new
                {
                    TotalAccess = g.Sum(c => c.AccessCount),
                    AvgAccessPerChunk = g.Average(c => c.AccessCount),
                    MostUsedChunk = g.OrderByDescending(c => c.AccessCount).Take(3).Select(c => new
                    {
                        c.Id,
                        c.AccessCount,
                        Source = c.Source.Title,
                    })
                }).FirstOrDefaultAsync();

            var sourceStats = await dbContext.Sources.Select(s => new
            {
                s.Type,
                Count = s.KnowledgeChunks.Count(),
            }).GroupBy(s => s.Type).Select(c => new
            {
                Type = c.Key,
                Count = c.Sum(x => x.Count),
                Sources = c.Count()
            }).ToListAsync();

            return Ok(new
            {
                TotalChunks = totalChunks,
                TotalSources = totalSources,
                TotalQueries = totalQueries,
                AccessStats = accessStats,
                SourceStats = sourceStats
            });
        }

        public static int GetWeekNumber(DateTimeOffset date)
        {
            var jan1 = new DateTime(date.Year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var firstThursday = jan1.AddDays(daysOffset);
            var cal = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            var weekNumber = cal.GetWeekOfYear(firstThursday, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weekNumber;
        }
    }
}
