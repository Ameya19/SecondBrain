using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class IngestionJob
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public string Status { get; set; } = "pending"; // "pending", "in_progress", "completed", "failed"
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public int ChunksCreated { get; set; }

        public Source Source { get; set; } = null!;
    }
}
