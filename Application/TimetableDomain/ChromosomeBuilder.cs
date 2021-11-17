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
        private DbSet<Course> dataCourses;
        private DbSet<Class> dataClasses;
        

        static TimeSpan RandomStartTime()
        {
            return TimeSpan.FromMilliseconds(Random.Next((int) TimeSpan.FromHours(9).TotalMilliseconds,
                (int) TimeSpan.FromHours(17).TotalMilliseconds));
        }


        public List<TimeSlotChromosome> Value;

        public TimeTableChromosome(DbSet<Course> courses, DbSet<Class> classes)
        {
            dataCourses = courses;
            dataClasses = classes;
            Generate();
        }
        public TimeTableChromosome(List<TimeSlotChromosome> slots, DbSet<Course> courses, DbSet<Class> classes)
        {
            dataCourses = courses;
            dataClasses = classes;
            Value = slots.ToList();
        }
        public override void Generate()
        {
            IEnumerable<TimeSlotChromosome> generateRandomSlots()
            {
                var courses = dataCourses
                    .Include(course => course.Teacher)
                    .Include(course => course.Groups).ToList();

                foreach (var course in courses)
                {
                    yield return new TimeSlotChromosome()
                    {
                        Group = course.Groups.OrderBy(group => Guid.NewGuid()).FirstOrDefault().GroupNumber,
                        CourseId = course.Id,
                        StartAt = RandomStartTime(),
                        PlaceId = dataClasses.OrderBy(_class => Guid.NewGuid()).FirstOrDefault().Id,
                        TeacherId = course.Teacher.Id,
                        Day=Random.Next(1,5)
                    };
                }
            }

            Value= generateRandomSlots().ToList();
            
        }

        public override IChromosome CreateNew()
        {
            var timeTableChromosome = new TimeTableChromosome(dataCourses, dataClasses);
            timeTableChromosome.Generate();
            return timeTableChromosome;
        }

        public override IChromosome Clone()
        {
            return new TimeTableChromosome(Value, dataCourses, dataClasses);
        }

        public override void Mutate()
        {
            var index = Random.Next(0, Value.Count - 1);
            var timeSlotChromosome = Value.ElementAt(index);
            timeSlotChromosome.StartAt = RandomStartTime();
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

        public class FitnessFunction : IFitnessFunction
        {
            public double Evaluate(IChromosome chromosome)
            {
                double score = 1;
                var values = (chromosome as TimeTableChromosome).Value;
                var GetoverLaps = new Func<TimeSlotChromosome, List<TimeSlotChromosome>>(current => values
                    .Except(new []{current})
                    .Where(slot=>slot.Day == current.Day)
                    .Where(slot =>slot.StartAt == current.StartAt 
                                  || slot.StartAt <= current.EndAt&&slot.StartAt >= current.StartAt
                                  || slot.EndAt >= current.StartAt&&slot.EndAt <= current.EndAt)
                    .ToList());

                
               

                foreach (var value in values)
                {
                    var overLaps= GetoverLaps(value);
                    score -= overLaps.GroupBy(slot => slot.TeacherId).Sum(x=>x.Count()-1);
                    score -= overLaps.GroupBy(slot => slot.PlaceId).Sum(x => x.Count()-1);
                    score -= overLaps.GroupBy(slot => slot.CourseId).Sum(x => x.Count()-1);
                    score -= overLaps.GroupBy(slot => slot.Group).Sum(x => x.Count()-1);
                    //score -= overLaps.Sum(item => item.Group.Intersect(value.Groups).Count());
                }

                score -= values.GroupBy(v => v.Day).Count() * 0.5;
                return Math.Pow(Math.Abs(score),-1);
            }
        }
    }
}