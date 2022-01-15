using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using NUnit.Framework;

namespace TimetableDomain
{
    [TestFixture]
    public class AlgorithmTests
    {
        private readonly IEnumerable<IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>> algorithms;
        private readonly List<Course> fiveSameCourses = Enumerable.Range(0, 5).Select(x => new Course()
        {
            Group = "FirstGroup",
            Place = "Place 1",
            Teacher = "Teacher 1",
            Title = "Course 1"
        }).ToList();

        public AlgorithmTests(IEnumerable<IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>> algorithms)
        {
            this.algorithms = algorithms;
        }
        
        [Test]
        public void RegularInputNoFilters()
        {
            var input = new AlgoritmInput()
            {
                Courses = fiveSameCourses,
                LessonLengthMinutes = 90,
                LessonStarts = new List<TimeSpan>() {new TimeSpan(1, 30, 0)},
                TeacherFilters = Array.Empty<Teacher>()
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
            
            var lessonStarts = new[] {input.LessonStarts};
            foreach (var timeslot in timeslots)
            {
                Assert.Contains(timeslot.Start, lessonStarts, $"Start time for course is not valid: {timeslot.Start}");
                Assert.Contains(timeslot.End - TimeSpan.FromMinutes(input.LessonLengthMinutes), lessonStarts, 
                    $"End time with lesson duration {input.LessonLengthMinutes} minutes is not valid: {timeslot.End}");
                AssertNoNonexistentCourses(groupToCourses, timeslot);
                AssertNoUnaccountedCourses(groupToCourses);
            }
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