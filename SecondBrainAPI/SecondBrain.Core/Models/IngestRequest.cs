using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Models
{
    public record IngestRequest(
        string Content,
        string Title,
        string Type,
        string? Url,
        DateTimeOffset? PublishedAt,
        string[]? Tags
    );
}
