using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TimetableDomain
{
    public class AlgoritmInput
    {
        public IEnumerable<Course> Courses { get; init; }
        public IEnumerable<Teacher> TeacherFilters { get; init; }
        public IEnumerable<TimeSpan> LessonStarts { get; init; }
        public int LessonLengthMinutes { get; init; }
    }
}