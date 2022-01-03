using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class Course
    {
        public string Title { get; init; }
        public string Teacher { get; init; }
        public string Place { get; init; }
        public string Group { get; set; }
    }
}