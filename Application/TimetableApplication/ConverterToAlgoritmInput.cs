using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class ConverterToAlgoritmInput
    {
        public AlgoritmInput Convert(IEnumerable<SlotInfo> inputCourses, IEnumerable<Filter> filters,
            IEnumerable<TimeSpan> lessonStarts, int lessonDurationMinutes)
        {
            var courses = inputCourses
                .Select(x => new Course()
                {
                    Title = x.Course,
                    Teacher = x.Teacher,
                    Group = x.Group,
                    Place = x.Room
                });
            
            var teachers = filters
                .Select(x => x.DaysCount.HasValue
                    ? new Teacher(x.Name, x.DaysCount.Value) 
                    : new Teacher(x.Name, x.Days));
            
            return new AlgoritmInput()
            {
                Courses = courses,
                TeacherFilters = teachers,
                LessonStarts = lessonStarts,
                LessonLengthMinutes = lessonDurationMinutes > 0 ? lessonDurationMinutes : 90
            };
        }
    }
}