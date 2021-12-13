using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    //public abstract class Entity
    //{
    //    public string Id { get; set; }
    //}
        
    public class Course
    {
        public string Title { get; set; }
        public string Teacher { get; set; }
        public List<string> Groups { get; set; }
    }
}