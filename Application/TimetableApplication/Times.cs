using System.Collections.Generic;
using System;

namespace TimetableApplication
{
    public class Times
    {
        public int Duration { get; init; }
        public List<TimeSpan> LessonStarts { get; init; }
    }
}
