using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class AlgorithmChooser
    {
        private readonly IReadOnlyDictionary<string, ITimetableMaker> timetableMakers;
        
        public AlgorithmChooser(IEnumerable<ITimetableMaker> algorithms)
        {
            timetableMakers = algorithms.ToDictionary(x => x.Algorithm.Name);
        }

        public ITimetableMaker ChooseAlgorithm(string name)
            => timetableMakers[name];
    }
}