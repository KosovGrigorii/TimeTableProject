using System.Collections.Generic;
using System.Linq;

namespace TimetableDomain
{
    public class Teacher
    {
        public string Name { get;  }
        public HashSet<int> WorkingDays { get; }
        public int DaysCount { get; }

        public Teacher(string name, int daysCount)
        {
            Name = name;
            WorkingDays = new HashSet<int>();
            DaysCount = daysCount;
        }
        
        public Teacher(string name, IEnumerable<int> days)
        {
            Name = name;
            WorkingDays = days.ToHashSet();
        }
        
        public bool IsDayForbidden(int day)
        {
            var isInSet = WorkingDays.Contains(day);
            if (!isInSet && WorkingDays.Count < DaysCount)
            {
                WorkingDays.Add(day);
                return false;
            }

            return !isInSet;
        }
    }
}