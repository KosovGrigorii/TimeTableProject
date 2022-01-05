using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public interface ITimetableMaker
    {
        Algorithm Algorithm { get; }
        IEnumerable<TimeSlot> GetTimetable(AlgoritmInput input);
    }
}