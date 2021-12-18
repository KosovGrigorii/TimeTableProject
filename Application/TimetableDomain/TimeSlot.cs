using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class TimeSlot
    {
        public DayOfWeek Day { get; }
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public string Place { get; }
        public string Course { get; }
        public string Teacher { get; }
        public List<string> Groups { get; }

        public TimeSlot(DayOfWeek day, TimeSpan start, TimeSpan end, string place,
            string course, string teacher, List<string> groups)
        {
            Day = day;
            Start = start;
            End = end;
            Place = place;
            Course = course;
            Teacher = teacher;
            Groups = groups;
        }
    }
}