using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public interface ITimetableMaker
    {
        Algorithm Name { get; }
        IEnumerable<TimeSlot> GetTimetable(AlgoritmInput input);
    }
}