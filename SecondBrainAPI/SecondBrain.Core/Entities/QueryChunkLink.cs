using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class QueryChunkLink
    {
        public Guid QueryId { get; set; }
        public Guid ChunkId { get; set; }
        public float RelevanceScore { get; set; } // Optional: score to indicate relevance of the chunk to the query
        public Query Query { get; set; } = null!;
        public KnowledgeChunk Chunk { get; set; } = null!;
    }
}
