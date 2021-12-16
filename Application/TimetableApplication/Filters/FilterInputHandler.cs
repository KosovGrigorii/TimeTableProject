

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
            var uid = "sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss";
            if (filterType == "Teacher")
                return UserToData.GetInputInfo(uid).Select(x => x.Teacher).Distinct();
            else if(filterType == "Group")
                return UserToData.GetInputInfo(uid).Select(x => x.Group).Distinct();
            throw new ArgumentException("No such filter");
        }
    }
}