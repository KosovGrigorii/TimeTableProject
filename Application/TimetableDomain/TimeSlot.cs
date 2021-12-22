using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class TimeSlot
    {
        public DayOfWeek Day { get; }
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        //public string Place { get; }
        public Course Course { get; }
        public string Teacher { get; }
        public string Group { get; }

        public TimeSlot(DayOfWeek day, TimeSpan start, TimeSpan end, Course course, string teacher, string group)
        {
            Day = day;
            Start = start;
            End = end;
            //Place = place;
            Course = course;
            Teacher = teacher;
            Group = group;
        }
    }
}