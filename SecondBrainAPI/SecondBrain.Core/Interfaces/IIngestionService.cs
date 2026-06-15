using SecondBrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Interfaces
{
    public interface IIngestionService
    {
        Task<Guid> IngestAsync(string content, SourceMetadata metadata);
    }
}
