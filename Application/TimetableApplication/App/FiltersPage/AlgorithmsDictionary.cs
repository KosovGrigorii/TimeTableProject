using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class AlgorithmsDictionary
    {
        private readonly IEnumerable<ITimetableMaker> algorithms;

        public AlgorithmsDictionary(IEnumerable<ITimetableMaker> algorithms)
        {
            this.algorithms = algorithms;
        }
        
        public IEnumerable<string> GetAlgorithms()
            => algorithms.Select(a => a.Algorithm.Name);

        public IReadOnlyDictionary<string, ITimetableMaker> GetDictionary()
            => algorithms.ToDictionary(a => a.Algorithm.Name);
    }
}