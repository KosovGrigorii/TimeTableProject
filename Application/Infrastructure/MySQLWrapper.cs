using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class MySQLWrapper<TKey, TValue> : IDatabaseWrapper<TKey, TValue> where TValue : MySQLValue<TKey>
    {
        public Database BaseName => Database.MySQL;
        private readonly MySQLContext<TKey, TValue> context;

        public MySQLWrapper(MySQLContext<TKey, TValue> context)
        {
            this.context = context;
        }
        
        public void AddTablesInfo(string valueTableName)
            => context.AddTablesInfo(valueTableName);

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