using System.Collections.Generic;
using Infrastructure;

namespace TimetableApplication
{
    public class FilterChecker
    {
        public static IEnumerable<string> GetFilterInfo()
        {
            return DBShell.GetTeachers();
            //return DBShell.GetGroups();
        }

        public static void AddFilters(List<Filter> filters)
        {
            DBShell.AddFilters();
        }
    }
}