using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class Entity
    {
        public string Id { get; set; }
    }
        
    public class Course : Entity
    {
        public string Title { get; set; }
        //public List<TimeSlot> Slots { get; set; }// хз нужны ли
        //public List<Teacher> Teachers { get; set; }
        public Teacher Teacher { get; set; }
        public List<Group> Groups { get; set; }
    }
        
    public class Class : Entity
    {
        public int RoomNumber { get; set; }
        //public List<TimeSlot> TimeSlots { get; set; }// промежутки доступа класса тоже под вопросом
    }
        
    public class Group : Entity
    {
        public string GroupNumber { get; set; }
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