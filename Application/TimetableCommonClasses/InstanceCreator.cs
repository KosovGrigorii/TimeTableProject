using System;
using System.Collections.Generic;

namespace TimetableCommonClasses
{
    public class InstanceCreator
    {
        public Filter GetFilter(string category, string name, int daysCount)
        {
            return new Filter(category, name, daysCount);
        }

        public TimeSlot GetTimeSlotTimeSlot(DayOfWeek day, TimeSpan start, TimeSpan end, string place,
            string course, string teacher, List<string> groups)
        {
            return new TimeSlot(day, start, end, place, course, teacher, groups);
        }
    }
}