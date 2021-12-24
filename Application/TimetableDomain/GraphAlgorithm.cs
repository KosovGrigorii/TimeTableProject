using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;

namespace TimetableDomain
{
    public class GraphAlgorithm : ITimetableMaker
    {
        public FilterHandler Handler = new FilterHandler();
        public Algorithm Name => Algorithm.Graph;
        public IEnumerable<TimeSlot> GetTimetable(IEnumerable<Course> courses, IEnumerable<Teacher> teachers, IEnumerable<TimeSpan> lessonStarts)
        {
            var CourseTime = new Dictionary<int, List<(Course, TimeSpan, bool)>>();
            var TeacherTime = new Dictionary<string, List<(int, TimeSpan)>>();
            var TeachersFilter = new Dictionary<string, Teacher>();
            var GroupTime = new Dictionary<string, List<(int, TimeSpan)>>();
            foreach (var course in courses)
            {
                if(!TeacherTime.ContainsKey(course.Teacher))
                    TeacherTime.Add(course.Teacher, new List<(int, TimeSpan)>());
                if(!GroupTime.ContainsKey(course.Groups[0])) 
                    GroupTime.Add(course.Groups[0], new List<(int, TimeSpan)>());
            }

            foreach (var teacher in teachers)
            {
                TeachersFilter.Add(teacher.Name, teacher);
            }

            foreach (var course in courses)
            {
                var isTransformable = TeachersFilter.ContainsKey(course.Teacher);
                var attemptResult = findAvailablePosition(course, CourseTime, TeacherTime, GroupTime,
                    lessonStarts, TeachersFilter, isTransformable);

                if (!attemptResult)
                {
                    var transformedCourse = Handler.handleFilters(TeachersFilter[course.Teacher].WorkingDays, course,
                        CourseTime);
                                
                    if (!transformedCourse.Equals(null))
                    {
                        findAvailablePosition(transformedCourse, CourseTime, TeacherTime,
                            GroupTime, lessonStarts, TeachersFilter);
                    }
                    else
                    {
                        findAvailablePosition(course, CourseTime, TeacherTime,
                            GroupTime, lessonStarts, TeachersFilter);
                    }
                }
            }
            
            
            var timeTable = CourseTime.SelectMany(chromosome => chromosome.Value, 
                (day, courseInfo) => new { day.Key, courseInfo }
                ).Select(chromosome =>
                
                new TimeSlot(
                    (DayOfWeek)chromosome.Key,
                    chromosome.courseInfo.Item2,
                    chromosome.courseInfo.Item2.Add(TimeSpan.FromHours(1.5)),
                    //chromosome.Place, 
                    chromosome.courseInfo.Item1,
                    chromosome.courseInfo.Item1.Teacher,
                    chromosome.courseInfo.Item1.Groups));
            return timeTable;
        }

        public bool findAvailablePosition(Course course, 
            Dictionary<int, List<(Course, TimeSpan, bool)>> CourseTime,
            Dictionary<string, List<(int, TimeSpan)>> TeacherTime,
            Dictionary<string, List<(int, TimeSpan)>> GroupTime,
            IEnumerable<TimeSpan> lessonStarts,
            Dictionary<string, Teacher> TeachersFilter,
            bool isTransformable = true)
        {
            for (int day = 1; day <= 5; day++)
            {
                var flag = false;
                foreach (var time in lessonStarts)
                {
                    if (TeacherTime[course.Teacher].Contains((day, time)) ||
                        GroupTime[course.Groups[0]].Contains((day, time))) continue;

                    if (!(isTransformable || Handler.checkPossibility(course, TeachersFilter, day))) return false;

                    if (CourseTime.ContainsKey(day))
                    {
                        CourseTime[day].Add((course, time, isTransformable));
                    }

                    else
                    {
                        CourseTime.Add(day, new List<(Course, TimeSpan, bool)>{(course, time, isTransformable)});
                    }
                        
                    TeacherTime[course.Teacher].Add((day, time));
                    GroupTime[course.Groups[0]].Add((day, time));
                    flag = true;
                    break;
                }
                    
                if (flag) return true;
            }

            return false;
        }
    }
}