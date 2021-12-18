using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;


namespace TimetableApplication
{
    public class TimetableMakingController
    {
        private readonly IReadOnlyDictionary<TimetableMakers, ITimetableMaker> timetableMakers;
        
        public TimetableMakingController(IReadOnlyDictionary<TimetableMakers, ITimetableMaker> timetableMakers)
        {
            this.timetableMakers = timetableMakers;
        }
        
        public void StartMakingTimeTable(string uid, 
            IUserData userToData, IEnumerable<Filter> filters)
        {
            var algorithm = timetableMakers[TimetableMakers.Genetic];
            
            var lessonStarts = new List<TimeSpan>() { 
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 40, 0)};
            var courses = userToData.GetInputInfo(uid)
                .Select(x => new Course()
                {
                    Title = x.Course,
                    Teacher = x.Teacher,
                    Groups = new List<string>() {x.Group}
                });
            var teachers = filters.Select(x => new Teacher(x.Name, x.Days)).ToList();
            var rooms = userToData.GetInputInfo(uid).Select(x => x.Class).ToList();
            
            var timeslots = algorithm.Start(courses, rooms, teachers, lessonStarts);
            userToData.SetTimeslots(uid, timeslots);
        }
    }
}