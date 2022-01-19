using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class ImplementationSelector<TIn, TOut, TImplementation> where TImplementation : IImplementation<TIn, TOut>
    {
        private readonly IReadOnlyDictionary<string, TImplementation> implementatinsDict;
        
        public ImplementationSelector(IEnumerable<TImplementation> implementatinsDict)
        {
            this.implementatinsDict = implementatinsDict.ToDictionary(x => x.Name);
        }

        public IEnumerable<string> GetTypes()
            => implementatinsDict.Keys;

        public TOut GetResult(string name, TIn parameters)
            => implementatinsDict[name].GetResult(parameters);
    }
}