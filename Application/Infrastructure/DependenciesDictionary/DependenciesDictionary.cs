using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class DependenciesDictionary<TIn, TOut, TDictionaryType> where TDictionaryType : IDictionaryType<TIn, TOut>
    {
        private readonly IReadOnlyDictionary<string, TDictionaryType> implementatinsDict;
        
        public DependenciesDictionary(IEnumerable<TDictionaryType> implementatinsDict)
        {
            this.implementatinsDict = implementatinsDict.ToDictionary(x => x.Name);
        }

        public IEnumerable<string> GetTypes()
            => implementatinsDict.Keys;

        public TOut GetResult(string name, TIn parameters)
            => implementatinsDict[name].GetResult(parameters);
    }
}