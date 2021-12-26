using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MySQLContext<TKey, TValue> : DbContext where TValue : DatabaseEntity<TKey>
    {
        public DbSet<TValue> Table { get; set; }

        public MySQLContext(DbContextOptions<MySQLContext<TKey, TValue>> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tableName = typeof(TValue).ToString().Split('.').Last() + 's';
            modelBuilder.Entity<TValue>().ToTable(tableName);
            base.OnModelCreating(modelBuilder);
        }  
    }
}