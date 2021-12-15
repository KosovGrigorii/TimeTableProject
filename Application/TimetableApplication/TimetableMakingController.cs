using System;
using System.Collections.Generic;
using System.Linq;
using Accord;
using TimetableDomain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using TimeSlot = TimetableDomain.TimeSlot;


namespace TimetableApplication
{
    public class TimetableMakingController
    {
        
        public static void StartMakingTimeTable(string uid, ITimetableMaker algorithm, IEnumerable<Filter> filters)
        {
            var lessonStarts = new List<TimeSpan>() { new TimeSpan(9, 0, 0),
                new TimeSpan(10, 40, 0)};
            var courses = UserToData.GetInputInfo(uid)
                .Select(x => new Course()
                {
                    Title = x.Course,
                    Teacher = x.Teacher,
                    Groups = new List<string>() {x.Group}
                }).ToList();

            var teachers = filters
                .Select(x => new Teacher(x.Name, x.Days)).ToList();
            
            var classes = UserToData.GetInputInfo(uid).Select(x => x.Class).ToList();
            var timeslots = algorithm.Start(courses, classes, teachers, lessonStarts);
            UserToData.SetTimeslots(uid, timeslots);
        }
    }
}