using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class Algorithms
    {
        public IEnumerable<string> NamesList { get; }
        
        public Algorithms(IEnumerable<ITimetableMaker> list)
        {
            NamesList = list.Select(a => a.Algorithm.Name);
        }
    }
}