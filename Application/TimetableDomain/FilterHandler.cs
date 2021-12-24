using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    public class FilterHandler
    {
        public bool checkPossibility(Course course,
            Dictionary<string, Teacher> TeachersFilter,
            int day)
        {
            if (TeachersFilter.ContainsKey(course.Teacher))
            {
                if (TeachersFilter[course.Teacher].IsDayForbidden(day))
                {
                    return false;
                }
            }

            return true;
        }


        public Course handleFilters(HashSet<int> workingDays, Course course, Dictionary<int, List<(Course, TimeSpan, bool)>> courseTime)
        {
            foreach (var workingDay in workingDays)
            {
                int index = courseTime[workingDay].FindIndex(TimeSlot => TimeSlot.Item3);

                if (index != -1)
                {
                    var transformedCourse = courseTime[workingDay][index].Item1;
                    courseTime[workingDay][index] = (course, courseTime[workingDay][index].Item2, false);
                    return transformedCourse;
                }
            }

            return null;
        }
    }
}