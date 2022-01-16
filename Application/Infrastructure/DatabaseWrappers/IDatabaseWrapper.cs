using System.Collections.Generic;

namespace Infrastructure
{
    public interface IDatabaseWrapper<in TKey, TStoredObject>
    {
        void AddRange(TKey key, IEnumerable<TStoredObject> content);
        bool ContainsKey(TKey key);
        IEnumerable<TStoredObject> ReadBy(TKey key);
    }
}