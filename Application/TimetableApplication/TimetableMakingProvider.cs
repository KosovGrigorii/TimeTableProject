using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using TimetableDomain;


namespace TimetableApplication
{
    public class TimetableMakingProvider
    {
        private AlgorithmChooser chooser;
        
        public TimetableMakingProvider(AlgorithmChooser chooser)
        {
            this.chooser = chooser;
        }
        
        public void StartMakingTimeTable(string uid, 
            DatabaseProvider database,
            IEnumerable<Filter> filters)
        {
            var algorithm = chooser.ChooseAlgorithm(Algorithm.Graph);
            
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
			var teachers = filters
                .Where(x => x.DaysCount.HasValue)
                .Select(x => new Teacher(x.Name, x.DaysCount.Value))
                .ToList();
            
			var timeslots = algorithm.GetTimetable(courses, teachers, lessonStarts);
			database.SetTimeslots(uid, timeslots);
        }
    }
}