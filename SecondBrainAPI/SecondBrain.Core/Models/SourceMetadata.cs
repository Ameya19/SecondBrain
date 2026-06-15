using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Models
{
    public record SourceMetadata
    (
        string Title,
        string Type,
        string? Url,
        DateTimeOffset? PublishedAt,
        string[]? Tags
    );
}
