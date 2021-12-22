using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class TimeSlot
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Place { get; set; }
        public string Course { get; set; }
        public string Teacher { get; set; }
        public string Group { get; set; }
    }
}