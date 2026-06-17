using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Models
{
    public record QueryResult(
        string Answer,
        List<SourceRef> Sources,
        List<string> Contradictions
    );

    public record SourceRef(
        string Title,
        DateTimeOffset IngestedAt,
        DateTimeOffset? PublishedAt
    );
}
