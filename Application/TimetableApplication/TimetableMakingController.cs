using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;
using TimetableCommonClasses;


namespace TimetableApplication
{
    public class TimetableMakingController
    {
        public static void StartMakingTimeTable(ITimetableMaker algorithm, IEnumerable<Filter> filters)
        {
            var lessonStarts = new List<TimeSpan>() { new TimeSpan(9, 0, 0),
                new TimeSpan(10, 40, 0)};
            var classes = DB.Slots.Select(x => x.Class);
            var teachers = filters.Select(x => new Teacher(x.Name, x.Days));
            var courses = DB.Slots
                .Select(x => new Course()
                {
                    Title = x.Course,
                    Teacher = x.Teacher,
                    Groups = new List<string>() {x.Group}
                });
            
            var timeslots = algorithm.Start(courses, classes, teachers, lessonStarts);
            DB.Timeslots = timeslots;
        }
    }
}