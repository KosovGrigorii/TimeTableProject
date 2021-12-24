using System;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class DatabaseTimeslot : DatabaseEbtity<string>
    {
        public int Day { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Place { get; set; }
        public string Course { get; set; }
        public string Teacher { get; set; }
        public string Group { get; set; }

        public DatabaseTimeslot() { }

        public DatabaseTimeslot(TimeSlot timeSlot, string uid)
        {
            Id = Guid.NewGuid().ToString();
            Day = (int)timeSlot.Day;
            Start = (int)timeSlot.Start.TotalMinutes;
            End = (int)timeSlot.End.TotalMinutes;
            Place = timeSlot.Place;
            Course = timeSlot.Course;
            Teacher = timeSlot.Teacher;
            Group = timeSlot.Group;
            KeyId = uid;
        }

        public TimeSlot ConvertToTimeslot()
        {
            return new TimeSlot()
            {
                Course = Course,
                Day = (DayOfWeek) Day,
                End = TimeSpan.FromMinutes(End),
                Group = Group,
                Place = Place,
                Start = TimeSpan.FromMinutes(Start),
                Teacher = Teacher
            };
        }
    }
}