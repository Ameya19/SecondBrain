using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class Source
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = ""; // "pdf", "url", "note", "youtube"
        public string Url { get; set; } = "";
        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset IngestedAt { get; set; }
        public string[]? Tags { get; set; }

        public ICollection<KnowledgeChunk> KnowledgeChunks { get; set; } = [];
        public ICollection<IngestionJob> IngestionJobs { get; set; } = [];
    }
}
