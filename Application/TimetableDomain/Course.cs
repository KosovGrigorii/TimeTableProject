using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class Course
    {
        public string Title { get; set; }
        public string Teacher { get; set; }
        public List<string> Groups { get; set; }
        public string Place { get; init; }
    }
}