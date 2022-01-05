using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class TimeSlot
    {
        public DayOfWeek Day { get; init; }
        public TimeSpan Start { get; init; }
        public TimeSpan End { get; init; }
        public string Place { get; init; }
        public string Course { get; init; }
        public string Teacher { get; init; }
        public string Group { get; init; }
    }
}