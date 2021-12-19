using Microsoft.EntityFrameworkCore;
using MySql.Data;

namespace TimetableApplication
{
    public class MysqlDataContext : DbContext
    {
        public DbSet<UserInputData> InputData { get; set; }
        public DbSet<UserTimeslots> Timeslots { get; set; }
        
        public MysqlDataContext(DbContextOptions<MysqlDataContext> options) : base(options) { } 
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {  
            // Use Fluent API to configure
        
            // Map entities to tables  
            modelBuilder.Entity<UserInputData>().ToTable("InputData");  
            modelBuilder.Entity<UserTimeslots>().ToTable("Timeslots");  
              
            // Configure Primary Keys  
            modelBuilder.Entity<UserInputData>().HasKey(x => x.Id).HasName("PK_InputData");  
            modelBuilder.Entity<UserTimeslots>().HasKey(t => t.Id).HasName("Timeslots"); 
            
            // Configure indexes  
            modelBuilder.Entity<UserInputData>().HasIndex(x => x.UserID).HasDatabaseName("Idx_InputUserID");  
            modelBuilder.Entity<UserTimeslots>().HasIndex(t => t.UserID).HasDatabaseName("Idx_TimeslotUserID");  
              
            // Configure columns  
            modelBuilder.Entity<UserInputData>().Property(x => x.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired(); 
            modelBuilder.Entity<UserInputData>().Property(x => x.UserID).HasColumnType("nvarchar(100)").IsRequired(false);  
            // modelBuilder.Entity<UserGroup>().Property(ug => ug.CreationDateTime).HasColumnType("datetime").IsRequired(false); 
            //   
            // modelBuilder.Entity<User>().Property(u => u.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();  
            // modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnType("nvarchar(50)").IsRequired();  
            // modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnType("nvarchar(50)").IsRequired();  
            // modelBuilder.Entity<User>().Property(u => u.UserGroupId).HasColumnType("int").IsRequired();  
            // modelBuilder.Entity<User>().Property(u => u.CreationDateTime).HasColumnType("datetime").IsRequired();  
            // modelBuilder.Entity<User>().Property(u => u.LastUpdateDateTime).HasColumnType("datetime").IsRequired(false);  
              
            // Configure relationships  
            //modelBuilder.Entity<User>().HasOne<UserGroup>()
            //.WithMany().HasPrincipalKey(ug => ug.Id).HasForeignKey(u => u.UserGroupId)
            //.OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Users_UserGroups");  
            
            base.OnModelCreating(modelBuilder);
        }  
    }
}