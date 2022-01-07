using System.Collections.Generic;
using TimetableDomain;

namespace TimetableApplication
{
    public class AlgorithmChooser
    {
        private readonly IReadOnlyDictionary<string, ITimetableMaker> timetableMakers;
        
        public AlgorithmChooser(AlgorithmsDictionary algorithmsDictionary)
        {
            timetableMakers = algorithmsDictionary.GetDictionary();
        }

        public ITimetableMaker ChooseAlgorithm(string name)
            => timetableMakers[name];
    }
}