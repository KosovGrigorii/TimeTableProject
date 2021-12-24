using System;
using System.Collections.Generic;

namespace TimetableApplication
{
    public class Filter
    {
        public string Name { get; }
        public int? DaysCount { get; }
        public List<DayOfWeek> Days { get; }

        public Filter(string name, int? daysCount, List<DayOfWeek> days)
        {
            Name = name;
            DaysCount = daysCount;
            Days = days;
        }
    }
}