using System;
using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseSlot : DatabaseEntity<string>
    {
        public string Course { get; init; }
        public string Group { get; init; }
        public string Teacher { get; init; }
        public string Room { get; init; }
    }
}