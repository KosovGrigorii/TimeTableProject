using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using TimetableDomain;
using System.IO;
using System.Linq;

namespace TimetableApplication
{
    [TestFixture]
    public class OutputFormatterTests
    {
        [Test]
        public void GetFile()
        {
            var file = new FileInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf"));
            Console.WriteLine(file.FullName);
            var formatter = new PdfOutputFormatter();
            formatter.MakeOutputFile(file.FullName, OutputConverter.ConvertTimeslotsToDictionary(GetSlots()));
            Process.Start(new ProcessStartInfo(file.FullName) { UseShellExecute = true });
        }
    
        private IEnumerable<TimeSlot> GetSlots()
        {
            var rows = new[]
            {
                "Monday 9:00 10:30 532 math1 mathteacher11 ft201",
                "Monday 9:00 10:30 612 math2 mathteacher21 ft203",
                "Monday 10:40 12:10 540 math2 mathteacher21 ft201",
                "Monday 10:40 12:10 513 math2 mathteacher22 ft202",
                "Monday 10:40 12:10 608 math1 mathteacher12 ft203",
                "Tuesday 10:40 12:10 526 prog1 progteacher11 ft201",
                "Tuesday 10:40 12:10 528 prog1 progteacher12 ft202",
                "Tuesday 10:40 12:10 150 prog2 progteacher23 ft203",
                "Tuesday 12:50 14:20 150 prog2 progteacher21 ft201",
                "Tuesday 12:50 14:20 513 math1 mathteacher21 ft202",
                "Tuesday 14:30 16:00 150 prog2 progteacher22 ft202",
                "Tuesday 14:30 16:00 532 math1 mathteacher12 ft204",
                "Wednesday 10:40 12:10 513 math2 mathteacher21 ft202",
                "Wednesday 12:50 14:20 513 math1 mathteacher12 ft204",
                "Thursday 12:50 14:20 0 prog1 progteacher01 ft202",
                "Thursday 12:50 14:20 0 prog1 progteacher02 ft203",
                "Friday 10:40 12:10 305 psyc1 psycteacher13 ft201",
                "Friday 10:40 12:10 306 psyc1 psycteacher24 ft202"
            };
            return rows.Select(GetTimeSlot);
        }
        
        private static readonly Dictionary<Tuple<string, string>, Course> Courses = 
            new ();
        
        private static Course GetCourse(string title, string teacher, string group)
        {
            var tuple = Tuple.Create(title, teacher);
            if (!Courses.ContainsKey(tuple))
                return GetNewCourse(title, teacher, group);
            var course = Courses[tuple];
            course.Group = group;
            return course;
        }
        
        private static Course GetNewCourse(string title, string teacher, string group)
        {
            var course = new Course { Title = title, Teacher = teacher, Group = group };
            if (teacher != null)
                Courses[Tuple.Create(title, teacher)] = course;
            return course;
        }
        
        private TimeSlot GetTimeSlot(string note)
        {
            var attributes = note.Split(' ');
            var day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), attributes[0]);
            var start = TimeSpan.Parse(attributes[1]);
            var end = TimeSpan.Parse(attributes[2]);
            var place = attributes[3];
            var title = attributes[4];
            var teacher = attributes[5];
            var group = attributes[6];
            var course = GetCourse(title, teacher, group);
            return new TimeSlot
            {
                Day = day, Start = start, End = end, Place = place, Course = course.Title, Teacher = teacher, Group = group
            };
        }
    }
}