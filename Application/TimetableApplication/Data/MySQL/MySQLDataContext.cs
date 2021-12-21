using Microsoft.EntityFrameworkCore;
using MySql.Data;

namespace TimetableApplication
{
    public class MySQLDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DBSlotInfo> SlotInfo { get; set; }
        public DbSet<DBTimeSlot> Timeslots { get; set; }
        
        public MySQLDataContext(DbContextOptions<MySQLDataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>()
                .HasMany(user => user.InputData)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(user => user.TimeSlots)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DBSlotInfo>().ToTable("SlotInfo"); 
            modelBuilder.Entity<DBTimeSlot>().ToTable("Timeslots");  
            base.OnModelCreating(modelBuilder);
        }  
    }
}