using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;
using Microsoft.EntityFrameworkCore;


namespace TimetableDomain
{
    class TimeTableChromosome : ChromosomeBase
    {
        //private readonly IEnumerable<Entity> _dataContext;
        static Random Random=new Random();
        private List<Course> dataCourses;
        private List<string> dataClasses;
        private List<TimeSpan> lessonStarts;
        

        private TimeSpan RandomStartTime()
        {
            return lessonStarts[Random.Next(lessonStarts.Count)];
        }


        public List<TimeSlotChromosome> Value;

        public TimeTableChromosome(List<Course> courses, List<string> classes, List<TimeSpan> lessonStarts)
        {
            dataCourses = courses;
            dataClasses = classes;
            this.lessonStarts = lessonStarts;
            Generate();
        }
        public TimeTableChromosome(List<TimeSlotChromosome> slots, List<Course> courses, List<string> classes,
            List<TimeSpan> lessonStarts)
        {
            dataCourses = courses;
            dataClasses = classes;
            this.lessonStarts = lessonStarts;
            Value = slots.ToList();
        }
        public override void Generate()
        {
            IEnumerable<TimeSlotChromosome> generateRandomSlots()
            {
                foreach (var course in dataCourses)
                {
                    yield return new TimeSlotChromosome()
                    {
                        Groups = course.Groups.ToList(),
                        //CourseId = course.Id,
                        StartAt = RandomStartTime(),
                        PlaceId = dataClasses.OrderBy(_class => Guid.NewGuid()).FirstOrDefault(),
                        TeacherId = course.Teacher, //Teacher themselves, but not id
                        Day=Random.Next(1,5)
                    };
                }
            }

            Value= generateRandomSlots().ToList();
            
        }

        public override IChromosome CreateNew()
        {
            var timeTableChromosome = new TimeTableChromosome(dataCourses, dataClasses,
                lessonStarts);
            timeTableChromosome.Generate();
            return timeTableChromosome;
        }

        public override IChromosome Clone()
        {
            return new TimeTableChromosome(Value, dataCourses, dataClasses, lessonStarts);
        }

        public override void Mutate()
        {
            var index = Random.Next(0, Value.Count - 1);
            var timeSlotChromosome = Value.ElementAt(index);
            timeSlotChromosome.StartAt = RandomStartTime(); //изменить срочно
            timeSlotChromosome.Day = Random.Next(1, 5);
            Value[index] = timeSlotChromosome;
            
        }

        public override void Crossover(IChromosome pair)
        {
            var randomVal = Random.Next(0, Value.Count - 2);
            var otherChromsome = pair as TimeTableChromosome;
            for (int index = randomVal; index < otherChromsome.Value.Count; index++)
            {
                Value[index] = otherChromsome.Value[index];
            }
        }
    }
}