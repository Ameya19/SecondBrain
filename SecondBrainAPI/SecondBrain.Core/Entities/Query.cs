using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Entities
{
    public class Query
    {
        public Guid Id { get; set; }
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
        public DateTimeOffset AskedAt { get; set; }
        public string ModelUsed { get; set; } = "";

        public ICollection<QueryChunkLink> QueryChunkLinks { get; set; } = [];

    }
}
