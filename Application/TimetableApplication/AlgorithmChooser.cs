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

        public ITimetableMaker ChooseAlgorithmForFilters(IEnumerable<Filter> filters)
        {
            if (filters.Any() && timetableMakers.TryGetValue(Algorithm.Graph, out var graph))
                return graph;
            return timetableMakers[Algorithm.Genetic];
        }
    }
}