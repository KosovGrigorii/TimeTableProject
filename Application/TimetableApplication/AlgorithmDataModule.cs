
using Microsoft.EntityFrameworkCore;
using TimetableDomain;
//using MySql.Data.MySqlClient;


namespace Infrastructure
{
    // public class DataContext : DbContext
    // {
    //     //public DbSet<StudentCourse> StudentCourse { get; set; }
    //
    //     public DbSet<Group> Groups { get; set; }
    //     public DbSet<Teacher> Teachers { get; set; }
    //     public DbSet<Course> Courses { get; set; }
    //     public DbSet<Class> Classes { get; set; }
    //     public DbSet<TimeSlot> TimeSlots { get; set; }
    //
    //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     {
    //         optionsBuilder.UseSqlServer(
    //             @"Server=(localdb)\ProjectsV13;Database=TimeTableDb;Integrated Security=True");
    //     }
    //
    //     protected override void OnModelCreating(ModelBuilder modelBuilder)
    //     {
    //         //modelBuilder.Entity<StudentCourse>().HasKey("StudentId", "CourseId");
    //         modelBuilder.Entity<Group>().HasMany(group => group.Courses);
    //         modelBuilder.Entity<Course>().HasMany(course => course.Groups);
    //         //modelBuilder.Entity<Course>().HasMany(course => course.Teachers);
    //         //modelBuilder.Entity<Group>().HasMany<TimeSlot>(group => group.TimeSlots);
    //         modelBuilder.Entity<TimeSlot>().HasMany<Group>(slot => slot.Groups);
    //
    //         base.OnModelCreating(modelBuilder);
    //     }
        
   // }
}