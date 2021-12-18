using System;
using System.Collections.Generic;
using TimetableCommonClasses;

namespace TimetableDomain
{
    public interface ITimetableMaker
    {
        TimetableMakers Name { get; }
        IEnumerable<TimeSlot> Start(IEnumerable<Course> cources, 
            IEnumerable<string> classes, IEnumerable<Teacher> teachers, IEnumerable<TimeSpan> lessonStarts);
    }
}