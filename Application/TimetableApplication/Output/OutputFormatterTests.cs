using System;
using System.Collections.Generic;
using NUnit.Framework;
using TimetableDomain;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TimetableCommonClasses;

namespace TimetableApplication
{
    [TestFixture]
    public class OutputFormatterTests
    {
        [Test]
        public void GetFile()
        {
            var formatter = new XlsxOutputFormatter();
            var slots = GetSlots();
            var file = formatter.GetOutputFile("output.xlsx", slots);
            file.MoveTo(".../new_file.xlsx");
        }

        [Test]
        public void CheckTime()
        {
            var formatter = new XlsxOutputFormatter();
            var slots = GetSlots();
            var timer = Stopwatch.StartNew();
            var file = formatter.GetOutputFile("output.xlsx", slots);
            timer.Stop();
            file.Delete();
            Console.WriteLine($"На создание файла было затрачено {timer.ElapsedMilliseconds} миллисекунд.");
        }

        private IEnumerable<TimeSlot> GetSlots()
        {
            var rows = new[]
            {
                "Monday 9:00 10:30 532 math1 mathteacher11 ft201 ft202",
                "Monday 9:00 10:30 612 math2 mathteacher21 ft203 ft204",
                "Monday 10:40 12:10 540 math2 mathteacher21 ft201",
                "Monday 10:40 12:10 513 math2 mathteacher22 ft202",
                "Monday 10:40 12:10 608 math1 mathteacher12 ft203 ft204",
                "Tuesday 10:40 12:10 526 prog1 progteacher11 ft201",
                "Tuesday 10:40 12:10 528 prog1 progteacher12 ft202",
                "Tuesday 10:40 12:10 150 prog2 progteacher23 ft203",
                "Tuesday 12:50 14:20 150 prog2 progteacher21 ft201",
                "Tuesday 12:50 14:20 513 math1 mathteacher21 ft202",
                "Tuesday 14:30 16:00 150 prog2 progteacher22 ft202",
                "Tuesday 14:30 16:00 532 math1 mathteacher12 ft204",
                "Wednesday 10:40 12:10 513 math2 mathteacher21 ft201 ft202 ft203 ft204",
                "Wednesday 12:50 14:20 513 math1 mathteacher12 ft203 ft204",
                "Thursday 12:50 14:20 0 prog1 progteacher01 ft201 ft202",
                "Thursday 12:50 14:20 0 prog1 progteacher02 ft203 ft204",
                "Friday 10:40 12:10 305 psyc1 psycteacher13 ft201 ft203",
                "Friday 10:40 12:10 306 psyc1 psycteacher24 ft202 ft204"
            };
            return rows.Select(GetTimeSlot);
        }
        
        private static readonly Dictionary<Tuple<string, string>, Course> Courses = 
            new Dictionary<Tuple<string, string>, Course>();
        
        private static Course GetCourse(string title, string teacher, List<string> groups)
        {
            var tuple = Tuple.Create(title, teacher);
            if (!Courses.ContainsKey(tuple))
                return GetNewCourse(title, teacher, groups);
            var course = Courses[tuple];
            foreach (var group in groups.Where(group => !course.Groups.Contains(group)))
                course.Groups.Add(group);
            return course;
        }
        
        private static Course GetNewCourse(string title, string teacher, List<string> groups)
        {
            var course = new Course { Title = title, Teacher = teacher, Groups = groups };
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
            var groups = attributes.Skip(6).ToList();
            //var groups = attributes.Skip(6).Select(GetGroup).ToList();
            var course = GetCourse(title, teacher, groups);
            return new TimeSlot(day, start, end, place, course.Title, course.Teacher, groups);
        }
    }
}