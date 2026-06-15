using Pgvector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class KnowledgeChunk
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = "";
        public Guid SourceId { get; set; }
        public DateTimeOffset IngestedAt { get; set; }
        public DateTimeOffset? LastAccessedAt { get; set; }
        public int AccessCount { get; set; }
        public Vector Embedding { get; set; } = null!;
        
        public Source Source { get; set; } = null!;
        public ICollection<ChunkTag> ChunkTags { get; set; } = [];
        public ICollection<QueryChunkLink> QueryChunkLinks { get; set; } = [];
        public ICollection<Contradiction> ContradictionsA { get; set; } = [];
        public ICollection<Contradiction> ContradictionsB { get; set; } = [];
    }
}
