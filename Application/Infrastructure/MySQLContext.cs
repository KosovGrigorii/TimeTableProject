using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MySQLContext<TKey, TValue> : DbContext where TValue : MySQLValue<TKey>
    {
        public DbSet<TValue> Table { get; set; }
        public string ValueTableName { get; private set; } 

        public MySQLContext(DbContextOptions<MySQLContext<TKey, TValue>> options) : base(options) { }

        public void AddTablesInfo(string valueTableName)
        {
            ValueTableName = valueTableName;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TValue>().ToTable(ValueTableName);
            base.OnModelCreating(modelBuilder);
        }  
    }
}