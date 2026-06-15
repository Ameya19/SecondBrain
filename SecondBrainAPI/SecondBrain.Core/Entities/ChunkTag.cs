using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class ChunkTag
    {
        public Guid ChunkId { get; set; }
        public Guid TagId { get; set; }

        public KnowledgeChunk Chunk { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
