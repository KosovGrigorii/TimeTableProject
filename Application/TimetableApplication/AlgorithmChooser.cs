using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class AlgorithmChooser
    {
        private readonly IReadOnlyDictionary<Algorithm, ITimetableMaker> timetableMakers;
        
        public AlgorithmChooser(IEnumerable<ITimetableMaker> algorithms)
        {
            timetableMakers = algorithms.ToDictionary(x => x.Name);
        }

        public ITimetableMaker ChooseFirstAlgorithm()
            => timetableMakers.First().Value;

        public ITimetableMaker ChooseAlgorithm(Algorithm name)
            => timetableMakers[name];
    }
}