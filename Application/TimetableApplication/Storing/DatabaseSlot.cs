using System;
using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseSlot : DatabaseEntity<string>
    {
        public string Course { get; set; }
        public string Group { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }
    }
}