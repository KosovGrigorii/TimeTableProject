using System.Collections.Generic;

namespace Infrastructure
{
    public interface IDatabaseWrapper<in TKey, TStoredObject>
    {
        void AddRange(TKey key, IEnumerable<TStoredObject> content);
        IEnumerable<TStoredObject> ReadBy(TKey key);
        void DeleteKey(TKey key);
    }
}