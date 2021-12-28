using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class MySQLWrapper<TKey, TValue> : IDatabaseWrapper<TKey, TValue> where TValue : DatabaseEntity<TKey>
    {
        private readonly MySQLContext<TKey, TValue> context;

        public MySQLWrapper(MySQLContext<TKey, TValue> context)
        {
            this.context = context;
        }
        
        public void AddRange(TKey key, IEnumerable<TValue> content)
        {
            context.Table.AddRange(content);
            context.SaveChanges();
        }

        public IEnumerable<TValue> ReadBy(TKey key)
        {
            return context.Table.Where(x => x.KeyId.Equals(key));
        }

        public void DeleteKey(TKey key)
        {
            context.Table.RemoveRange(context.Table.Where(x => x.KeyId.Equals(key)));
            context.SaveChanges();
        }
    }
}