using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Core.Models
{
    public record QueryRequest(
     string Question, 
     int? TopK
     );
    
}
