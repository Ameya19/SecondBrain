using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public DateTimeOffset CreatedAt { get; set; }
        
        public ICollection<ChunkTag> ChunkTags { get; set; } = [];
    }
}
