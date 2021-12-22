using System.Collections.Generic;

namespace Infrastructure
{
    public class MySQLMultipleValuesClass<TKey, TValue> where TValue : MySQLValue
    {
        public TKey Id { get; set; }
        public IList<TValue> Data { get; set; }
    }
}