using System.Collections.Generic;
using System;

namespace TimetableApplication
{
    public class Times
    {
        public int Duration { get; set; }
        public List<TimeSpan> LessonStarts { get; set; }
    }
}
