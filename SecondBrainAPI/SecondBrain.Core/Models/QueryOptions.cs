using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Models
{
    public record QueryOptions
    (
        int TopK = 5,
        bool UseTemporalReranking = true
    );
}
