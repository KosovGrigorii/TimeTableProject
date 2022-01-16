using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace TimetableDomain
{
    public class GraphAlgorithm : IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>
    {
        private readonly FilterHandler handler;
        public string Name => "Graph";

        public GraphAlgorithm(FilterHandler handler)
        {
            this.handler = handler;
        }
        
        public IEnumerable<TimeSlot> GetResult(AlgoritmInput parameters)
        {
            return GetTimetable(parameters);
        }
        
        private IEnumerable<TimeSlot> GetTimetable(AlgoritmInput input)
        {
            var teacherTime = input.Courses.Select(c => c.Teacher).Distinct()
                .ToDictionary(t => t, t => new List<(int, TimeSpan)>());
            var groupTime = input.Courses.Select(c => c.Group).Distinct()
                .ToDictionary(g => g, g => new List<(int, TimeSpan)>());
            var teachersFilter = input.TeacherFilters.ToDictionary(t => t.Name);
            
            var courseTime = new Dictionary<int, List<CourseTime>>();

            foreach (var course in input.Courses)
            {
                var attemptResult = FindAvailablePosition(course, courseTime, teacherTime, groupTime,
                    input.LessonStarts, teachersFilter);

                if (!attemptResult)
                {
                    var transformedCourse = handler.HandleFilters(teachersFilter[course.Teacher].WorkingDays, course,
                        courseTime);
                                
                    if (transformedCourse != null)
                    {
                        FindAvailablePosition(transformedCourse, courseTime, teacherTime,
                            groupTime, input.LessonStarts, teachersFilter);
                    }
                    else
                    {
                        teachersFilter.Remove(course.Teacher);
                        FindAvailablePosition(course, courseTime, teacherTime,
                            groupTime, input.LessonStarts, teachersFilter);
                    }
                }
            }

            var timeTable = courseTime
                .SelectMany(chromosome
                        => chromosome.Value, (day, courseInfo) 
                        => new { day.Key, courseInfo })
                .Select(chromosome =>
                    new TimeSlot
                    {
                        Day = (DayOfWeek)chromosome.Key,
                        Start = chromosome.courseInfo.Time,
                        End = chromosome.courseInfo.Time.Add(TimeSpan.FromMinutes(input.LessonLengthMinutes)),
                        Place = chromosome.courseInfo.Course.Place,
                        Course = chromosome.courseInfo.Course.Title,
                        Teacher = chromosome.courseInfo.Course.Teacher,
                        Group = chromosome.courseInfo.Course.Group
                    });

            return timeTable;
        }

        private bool FindAvailablePosition(Course course, 
            IDictionary<int, List<CourseTime>> courseTime,
            IReadOnlyDictionary<string, List<(int, TimeSpan)>> teacherTime,
            IReadOnlyDictionary<string, List<(int, TimeSpan)>> groupTime,
            IEnumerable<TimeSpan> lessonStarts,
            Dictionary<string, Teacher> teachersFilter)
        {
            var isTransformable = !teachersFilter.ContainsKey(course.Teacher);
            
            for (var day = 1; day <= 5; day++)
            {
                foreach (var time in lessonStarts)
                {
                    if (teacherTime[course.Teacher].Contains((day, time)) ||
                        groupTime[course.Group].Contains((day, time))) continue;

                    if (!(isTransformable || handler.CheckPossibility(course, teachersFilter, day))) break;

                    if (!courseTime.ContainsKey(day))
                        courseTime.Add(day, new List<CourseTime>());
                    courseTime[day].Add(new CourseTime(course, time, isTransformable));
                        
                    teacherTime[course.Teacher].Add((day, time));
                    groupTime[course.Group].Add((day, time));
                    return true;
                }
            }
            return false;
        }
    }
}