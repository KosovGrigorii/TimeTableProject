using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using NUnit.Framework;

namespace TimetableDomain
{
    [TestFixture]
    public class AlgorithmTests
    {
        private readonly IEnumerable<IImplementation<AlgoritmInput, IEnumerable<TimeSlot>>> algorithms = 
            new IImplementation<AlgoritmInput, IEnumerable<TimeSlot>>[]
        {
            new GeneticAlgorithm(new (), new ()),
            new GraphAlgorithm(new ())
        };
        private readonly List<Course> fiveSameCourses = Enumerable.Range(0, 5).Select(x => new Course()
        {
            Group = "FirstGroup",
            Place = "Place 1",
            Teacher = "Teacher 1",
            Title = "Course 1"
        }).ToList();
        
        [Test]
        public void RegularInputNoFilters()
        {
            var input = new AlgoritmInput()
            {
                Courses = fiveSameCourses,
                LessonLengthMinutes = 90,
                LessonStarts = new List<TimeSpan>() {new (9, 0, 0), new (10, 40, 0)},
                TeacherFilters = Array.Empty<Teacher>()
            };
            foreach(var algorithm in algorithms)
                AssertCreatesValidTimetable(input, algorithm.GetResult(input));
        }
        
        [Test]
        public void ImpossibleFilterHandler()
        {
            var input = new AlgoritmInput()
            {
                Courses = fiveSameCourses,
                LessonLengthMinutes = 90,
                LessonStarts = new List<TimeSpan>() {new (9, 0, 0), new (10, 40, 0)},
                TeacherFilters = new List<Teacher>(){new Teacher("Teacher 1", 1)}
            };
            foreach(var algorithm in algorithms)
                AssertCreatesValidTimetable(input, algorithm.GetResult(input));
        }

        private void AssertCreatesValidTimetable(AlgoritmInput input, IEnumerable<TimeSlot> timeslots)
        {
            var groupToCourses = new Dictionary<string, List<Course>>();
            foreach (var course in input.Courses)
            {
                if (!groupToCourses.TryGetValue(course.Group, out var courseList))
                {
                    courseList = new ();
                    groupToCourses[course.Group] = courseList;
                }
                courseList.Add(course);
            }

            var lessonStarts = input.LessonStarts.ToArray();
            foreach (var timeslot in timeslots)
            {
                Assert.Contains(timeslot.Start, lessonStarts, $"Start time for course is not valid: {timeslot.Start}");
                AssertNoNonexistentCourses(groupToCourses, timeslot);
            }
            AssertNoUnaccountedCourses(groupToCourses);
        }

        private void AssertNoNonexistentCourses(IReadOnlyDictionary<string, List<Course>> groupToCourses, TimeSlot timeslot)
        {
            if (!groupToCourses.TryGetValue(timeslot.Group, out var groupCourses))
                Assert.Fail($"Algorithm returned data for group {timeslot.Group} which does not exist");
            foreach (var course in groupCourses.Where(course => 
                course.Title == timeslot.Course && 
                course.Place == timeslot.Place &&
                course.Teacher == timeslot.Teacher))
            {
                groupToCourses[timeslot.Group].Remove(course);
                return;
            }
            Assert.Fail($"Course {timeslot.Course} for group {timeslot.Group} " +
                        $"with teacher {timeslot.Teacher} in classroom {timeslot.Place} " +
                        $"does not exist but was created by algorithm.");
        }
        
        private void AssertNoUnaccountedCourses(IReadOnlyDictionary<string, List<Course>> groupToCourses)
        => Assert.True(groupToCourses.Select(pair => pair.Value.Count).Sum() == 0, "Algorithm has generated timetable not for all courses");
    }
}