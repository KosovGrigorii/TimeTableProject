using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MySQLContext<TKey, TValue> : DbContext where TValue : MySQLValue
    {
        public DbSet<MySQLMultipleValuesClass<TKey, TValue>> KeyTable { get; set; }
        public string KeyTableName { get; private set; } 
        public string ValueTableName { get; private set; } 

        public MySQLContext(DbContextOptions<MySQLContext<TKey, TValue>> options) : base(options) { }

        public void AddTablesInfo(string keyTableName, string valueTableName)
        {
            KeyTableName = keyTableName;
            ValueTableName = valueTableName;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MySQLMultipleValuesClass<TKey, TValue>>().ToTable(KeyTableName);
            modelBuilder.Entity<TValue>().ToTable(ValueTableName);
            
            modelBuilder.Entity<MySQLMultipleValuesClass<TKey, TValue>>().HasKey(x => x.Id).HasName("Id");  
            modelBuilder.Entity<MySQLMultipleValuesClass<TKey, TValue>>()
                .HasMany(key => key.Data)
                .WithOne()
                .HasForeignKey(x => x.KeyId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }  
    }
}