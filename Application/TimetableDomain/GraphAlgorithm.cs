using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;

namespace TimetableDomain
{
    public class GraphAlgorithm : ITimetableMaker
    {
        public Algorithm Name => Algorithm.Graph;
        
        public IEnumerable<TimeSlot> GetTimetable(AlgoritmInput input)
        {
            var CourseTime = new Dictionary<int, List<(Course, TimeSpan, bool)>>();
            var TeacherTime = new Dictionary<string, List<(int, TimeSpan)>>();
            var TeachersFilter = new Dictionary<string, Teacher>();
            var GroupTime = new Dictionary<string, List<(int, TimeSpan)>>();
            foreach (var course in input.Courses)
            {
                if(!TeacherTime.ContainsKey(course.Teacher))
                    TeacherTime.Add(course.Teacher, new List<(int, TimeSpan)>());
                if(!GroupTime.ContainsKey(course.Group)) 
                    GroupTime.Add(course.Group, new List<(int, TimeSpan)>());
            }

            foreach (var teacher in input.TeacherFilters)
            {
                TeachersFilter.Add(teacher.Name, teacher);
            }

            foreach (var course in input.Courses)
            {
                for (int i = 1; i <= 5; i++)
                {
                    var flag = false;
                    foreach (var time in input.LessonStarts)
                    {
                        var isTransformable = true;
                        if (TeacherTime[course.Teacher].Contains((i, time)) || 
                            GroupTime[course.Group].Contains((i, time))) continue;
                        
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
                        GroupTime[course.Group].Add((i, time));
                        flag = true;
                        break;
                    }
                    
                    if (flag) break;
                }
            }

            var timeTable = CourseTime.SelectMany(chromosome => chromosome.Value,
                (day, courseInfo) => new {day.Key, courseInfo}
            ).Select(chromosome =>
                new TimeSlot()
                {
                    Day = (DayOfWeek) chromosome.Key,
                    Start = chromosome.courseInfo.Item2,
                    End = chromosome.courseInfo.Item2.Add(TimeSpan.FromMinutes(input.LessonLengthMinutes)),
                    Place = chromosome.courseInfo.Item1.Place,
                    Course = chromosome.courseInfo.Item1.Title,
                    Teacher = chromosome.courseInfo.Item1.Teacher,
                    Group = chromosome.courseInfo.Item1.Group
                });
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