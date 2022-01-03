using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public class DictionaryWrapper<TKey, TValue> : IDatabaseWrapper<TKey, TValue>
    {
        private readonly IDictionary<TKey, IEnumerable<TValue>> dictionary = new Dictionary<TKey, IEnumerable<TValue>>();

        public void AddRange(TKey key, IEnumerable<TValue> content)
        {
            if (!dictionary.ContainsKey(key))
                dictionary[key] = content;
            else
                throw new ArgumentException($"Key {key} is already added");
        }

        public IEnumerable<TValue> ReadBy(TKey key)
        {
            return dictionary[key];
        }

        public void DeleteKey(TKey key)
        {
            dictionary.Remove(key);
        }
    }
}