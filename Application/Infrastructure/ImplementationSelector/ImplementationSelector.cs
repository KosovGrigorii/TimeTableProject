using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class ImplementationSelector<TIn, TOut, TDictionaryType> where TDictionaryType : IImplementation<TIn, TOut>
    {
        private readonly IReadOnlyDictionary<string, TDictionaryType> implementatinsDict;
        
        public ImplementationSelector(IEnumerable<TDictionaryType> implementatinsDict)
        {
            this.implementatinsDict = implementatinsDict.ToDictionary(x => x.Name);
        }

        public IEnumerable<string> GetTypes()
            => implementatinsDict.Keys;

        public TOut GetResult(string name, TIn parameters)
            => implementatinsDict[name].GetResult(parameters);
    }
}