using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Interfaces
{
    public interface IEmbeddingService
    {
        Task<float[]> EmbedAsync(string text);
        Task<List<float[]>> EmbedBatchAsync(List<string> texts);
    }
}
