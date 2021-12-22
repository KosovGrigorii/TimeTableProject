using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;

namespace TimetableDomain
{
    public class GraphAlgorithm : ITimetableMaker
    {
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
                for (int i = 1; i <= 5; i++)
                {
                    var flag = false;
                    foreach (var time in lessonStarts)
                    {
                        var isTransformable = true;
                        if (TeacherTime[course.Teacher].Contains((i, time)) || 
                            GroupTime[course.Groups[0]].Contains((i, time))) continue;
                        
                        if (TeachersFilter.ContainsKey(course.Teacher))
                        {
                            if (TeachersFilter[course.Teacher].IsDayForbidden(i))
                            {
                                var transformedCourse = handleFilters(TeachersFilter[course.Teacher].WorkingDays, course,
                                    CourseTime);
                                if (!transformedCourse.Equals(null))
                                {
                                    
                                }
                            }
                            
                            isTransformable = false;
                        }

                        if (CourseTime.ContainsKey(i))
                        {
                            CourseTime[i].Add((course, time, isTransformable));
                        }

                        else
                        {
                            CourseTime.Add(i, new List<(Course, TimeSpan, bool)>{(course, time, isTransformable)});
                        }
                        
                        TeacherTime[course.Teacher].Add((i, time));
                        GroupTime[course.Groups[0]].Add((i, time));
                        flag = true;
                        break;
                    }
                    
                    if (flag) break;
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