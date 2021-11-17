using System;
using System.Collections.Generic;

namespace UserInterface
{
    public class Demo
    {
        public class Entity
        {
            public string Id { get; set; }
        }
        
        public class Course : Entity
        {
            public string Title { get; set; }
            public List<TimeSlot> Slots { get; set; }
            public List<Teacher> Teachers { get; set; }
            public List<Group> Groups { get; set; }
        }
        
        public class Class : Entity
        {
            public int RoomNumber { get; set; }
            public List<TimeSlot> TimeSlots { get; set; }
        }
        
        public class Group : Entity
        {
            public int GroupNumber { get; set; }
            public List<Course> Courses { get; set; }
            //public List<TimeSlot> TimeSlots { get; set; }
        }

        public class Teacher : Entity
        {
            public List<Course> Courses { get; set; }
            public string Name { get; set; }
        }
        
        public class TimeSlot : Entity
        {
            public DayOfWeek Day { get; set; }
            public TimeSpan Start { get; set; }
            public TimeSpan End { get; set; }
            //public string PlaceId { get; set; }
            public Class Place { get; set; }
            //public string CourseId { get; set; }
            public Course Course { get; set; }
            public List<Group> Groups { get; set; }
        }
    }
}