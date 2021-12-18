using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public interface ITimetableMaker
    {
        Algorithm Name { get; }
        IEnumerable<TimeSlot> GetTimetable(IEnumerable<Course> cources, 
            IEnumerable<string> classes, IEnumerable<Teacher> teachers, IEnumerable<TimeSpan> lessonStarts);
    }
}