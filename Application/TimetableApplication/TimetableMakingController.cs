using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;


namespace TimetableApplication
{
    public class TimetableMakingController
    {
        public void StartMakingTimeTable(string uid, ITimetableMaker algorithm, 
            IUserData userToData, IEnumerable<Filter> filters)
        {
            var lessonStarts = new List<TimeSpan>() { new TimeSpan(9, 0, 0),
                new TimeSpan(10, 40, 0)};
            var courses = userToData.GetInputInfo(uid)
                .Select(x => new Course()
                {
                    Title = x.Course,
                    Teacher = x.Teacher,
                    Groups = new List<string>() {x.Group}
                });
            var teachers = filters.Select(x => new Teacher(x.Name, x.Days)).ToList();
            var classes = userToData.GetInputInfo(uid).Select(x => x.Class).ToList();
            
            var timeslots = algorithm.Start(courses, classes, teachers, lessonStarts);
            userToData.SetTimeslots(uid, timeslots);
        }
    }
}