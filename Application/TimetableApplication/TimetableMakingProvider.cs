using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using TimetableDomain;


namespace TimetableApplication
{
    public class TimetableMakingProvider
    {
        private readonly IReadOnlyDictionary<Algorithm, ITimetableMaker> timetableMakers;
        
        public TimetableMakingProvider(IReadOnlyDictionary<Algorithm, ITimetableMaker> timetableMakers)
        {
            this.timetableMakers = timetableMakers;
        }
        
        public void StartMakingTimeTable(string uid, 
            DatabaseProvider database,
            IEnumerable<Filter> filters)
        {
            var algorithm = timetableMakers[Algorithm.Graph];
            
            var lessonStarts = new List<TimeSpan>() { 
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 40, 0)};
            var courses = database.GetInputInfo(uid)
                .Select(x => new Course()
                {
                    Title = x.Course,
                    Teacher = x.Teacher,
					Group = x.Group,
					Place = x.Room
				});
			var teachers = filters.Select(x => new Teacher(x.Name, x.Days)).ToList();
			var timeslots = algorithm.GetTimetable(courses, teachers, lessonStarts);
			database.SetTimeslots(uid, timeslots);
        }
    }
}