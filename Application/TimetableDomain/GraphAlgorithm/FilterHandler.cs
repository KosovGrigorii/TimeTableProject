using System.Collections.Generic;

namespace TimetableDomain
{
    public class FilterHandler
    {
        public bool CheckPossibility(Course course,
            Dictionary<string, Teacher> TeachersFilter,
            int day)
        {
            if (TeachersFilter.ContainsKey(course.Teacher))
                return (!TeachersFilter[course.Teacher].IsDayForbidden(day));
            return true;
        }


        public Course HandleFilters(HashSet<int> workingDays, Course course, Dictionary<int, List<CourseTime>> courseTime)
        {
            foreach (var workingDay in workingDays)
            {
                var index = courseTime[workingDay].FindIndex(TimeSlot => TimeSlot.IsTransformable);

                if (index != -1)
                {
                    var transformedCourse = courseTime[workingDay][index].Course;
                    courseTime[workingDay][index] = new CourseTime(course, courseTime[workingDay][index].Time, false);
                    return transformedCourse;
                }
            }

            return null;
        }
    }
}