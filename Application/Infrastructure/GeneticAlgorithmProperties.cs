using System;
using System.Collections.Generic;



namespace Infrastructure
{
    public class DbEntity
    {
        public string Id { get; set; }
    }

    public class Course : DbEntity
    {
        public string Title { get; set; }
        public List<TimeSlot> Slots { get; set; }
        public Teacher Teacher { get; set; }
        public List<StudentCourse> Students { get; set; }

    }

    public class Place : DbEntity
    {
        public string Name { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
    }

    public class Group : DbEntity
    {
        public string Name { get; set; }
        public List<StudentCourse> Courses { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
    }

    public class StudentCourse
    {
        public string CourseId { get; set; }
        public string StudentId { get; set; }

        public Group Group { get; set; }
        public Course Course { get; set; }

    }

    public class Teacher : DbEntity
    {
        public List<Course> Courses { get; set; }
        public string Name { get; set; }
    }

    public class TimeSlot : DbEntity
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string PlaceId { get; set; }
        public Place Place { get; set; }
        public string CourseId { get; set; }
        public Course Course { get; set; }
        public List<Group> Groups { get; set; }
    }
}