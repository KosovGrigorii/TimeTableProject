using System.Collections.Generic;

namespace Infrastructure
{
    public interface IDatabaseWrapper<TKey, TStoredObject>
    {
        Database BaseName { get; }
        void AddRange(TKey key, IEnumerable<TStoredObject> content);
        IEnumerable<TStoredObject> ReadBy(TKey key);
        void DeleteKey(TKey key);
    }
}