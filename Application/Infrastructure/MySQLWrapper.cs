using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class MySQLWrapper<TKey, TValue> : IDatabaseWrapper<TKey, TValue> where TKey : class where TValue : MySQLValue
    {
        public Database BaseName => Database.MySQL;
        private readonly MySQLContext<TKey, TValue> context;

        public MySQLWrapper(MySQLContext<TKey, TValue> context)
        {
            this.context = context;
        }
        
        public void AddTablesInfo(string keyTableName, string valueTableName)
            => context.AddTablesInfo(keyTableName, valueTableName);

        public void AddRange(TKey key, IEnumerable<TValue> content)
        {
            var addedEntity = new MySQLMultipleValuesClass<TKey, TValue>()
            {
                Id = key,
                Data = content.ToList()
            };
            context.KeyTable.Add(addedEntity);
            context.SaveChanges();
        }

        public IEnumerable<TValue> ReadBy(TKey key)
        {
            return context.KeyTable.Find(key).Data;
        }

        public void DeleteKey(TKey key)
        {
            context.KeyTable.Remove(context.KeyTable.Find(key));
            context.SaveChanges();
        }
    }
}