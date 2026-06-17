using SecondBrain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Interfaces
{
    public interface IQueryService
    {
        Task<QueryResult> QueryAsync(string question, QueryOptions? options = null);
    }
}
