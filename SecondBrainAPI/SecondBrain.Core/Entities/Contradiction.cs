using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class Contradiction
    {
        public Guid Id { get; set; }
        public DateTimeOffset DetectedAt { get; set; }
        public Guid ChunkAId { get; set; }
        public Guid ChunkBId { get; set; }
        public bool Resolved { get; set; }
        public string? ResolutionNote { get; set; }
        public KnowledgeChunk ChunkA { get; set; } = null!;
        public KnowledgeChunk ChunkB { get; set; } = null!;
    }
}
