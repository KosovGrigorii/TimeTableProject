using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;


namespace TimetableDomain
{
    class TimeTableChromosome : ChromosomeBase
    {
        public List<TimeSlotChromosome> Value { get; private set; }
        private static readonly Random random = new ();
        private readonly IEnumerable<Course> dataCourses;
        private readonly List<TimeSpan> lessonStarts;
        
        public TimeTableChromosome(IEnumerable<Course> courses, IEnumerable<TimeSpan> lessonStarts)
        {
            dataCourses = courses;
            this.lessonStarts = lessonStarts.ToList();
            Generate();
        }
        
        private TimeTableChromosome(List<TimeSlotChromosome> slots, IEnumerable<Course> courses,
            List<TimeSpan> lessonStarts)
        {
            dataCourses = courses;
            this.lessonStarts = lessonStarts;
            Value = slots.ToList();
        }

        private TimeSpan RandomStartTime()
        {
            return lessonStarts[random.Next(lessonStarts.Count)];
        }
        
        public override void Generate()
        {
            IEnumerable<TimeSlotChromosome> GenerateRandomSlots()
            {
                foreach (var course in dataCourses)
                {
                    yield return new TimeSlotChromosome()
                    {
                        Group = course.Group,
                        StartAt = RandomStartTime(),
                        Course = course,
                        Teacher = course.Teacher,
                        Day=random.Next(1,5)
                    };
                }
            }

            Value = GenerateRandomSlots().ToList();
        }

        public override IChromosome CreateNew()
        {
            var timeTableChromosome = new TimeTableChromosome(dataCourses,
                lessonStarts);
            timeTableChromosome.Generate();
            return timeTableChromosome;
        }

        public override IChromosome Clone()
        {
            return new TimeTableChromosome(Value, dataCourses, lessonStarts);
        }

        public override void Mutate()
        {
            var index = random.Next(0, Value.Count - 1);
            var timeSlotChromosome = Value.ElementAt(index);
            timeSlotChromosome.StartAt = RandomStartTime();
            timeSlotChromosome.Day = random.Next(1, 5);
            Value[index] = timeSlotChromosome;
            
        }

        public override void Crossover(IChromosome pair)
        {
            var randomVal = random.Next(0, Value.Count - 2);
            var otherChromsome = pair as TimeTableChromosome;
            for (int index = randomVal; index < otherChromsome.Value.Count; index++)
            {
                Value[index] = otherChromsome.Value[index];
            }
        }
    }
}