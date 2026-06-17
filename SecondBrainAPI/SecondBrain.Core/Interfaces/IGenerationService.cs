using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Interfaces
{
    public interface IGenerationService
    {
        Task<string> GenerateAsync(string prompt);
        IAsyncEnumerable<string> StreamAsync(string prompt);
    }
}
