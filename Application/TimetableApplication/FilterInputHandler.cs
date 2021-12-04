

using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class FilterInputHandler
    {
        public static IEnumerable<string> GetFilterTypes()
        {
            var filterTypes = new List<string>
            {
                "Teacher", 
                "Group"
            };
            return filterTypes;
        }

        public static IEnumerable<string> GetFiltersOfType(string filterType)
        {
            if (filterType == "Teacher")
                return DB.Slots.Select(x => x.Teacher);
            else if(filterType == "Group")
                return DB.Slots.Select(x => x.Group);
            throw new ArgumentException("No such filter");
        }
    }
}