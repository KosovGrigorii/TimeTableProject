using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;

namespace TimetableDomain
{
    public class FitnessFunction : IFitnessFunction
    {
        private Dictionary<string, Teacher> Teachers;

        public FitnessFunction(IEnumerable<Teacher> teachers)
        {
            Teachers = teachers.ToDictionary(x => x.Name);
        }
        
        
        public double Evaluate(IChromosome chromosome)
        {
            double score = 1;
            var values = (chromosome as TimeTableChromosome).Value;
            
            var getTimeOverLaps = new Func<TimeSlotChromosome, IEnumerable<TimeSlotChromosome>>(current => values
                .Except(new[] { current })
                .Where(slot => slot.IsOverlappedBy(current)));
            
            var teacherWorkDaysOverLaps = new Func<TimeSlotChromosome, IEnumerable<TimeSlotChromosome>>(current =>
                values.Where(x => 
                        Teachers.TryGetValue(x.Teacher, out var teacher) && teacher.IsDayForbidden(x.Day)));
            var GetoverLaps = new Func<TimeSlotChromosome, List<TimeSlotChromosome>>(current => 
                getTimeOverLaps(current)
                    .Concat(teacherWorkDaysOverLaps(current))
                    .ToList()
                );

            foreach (var value in values)
            {
                var overLaps = GetoverLaps(value);
                score -= overLaps.GroupBy(slot => slot.Teacher).Sum(x => x.Count() - 1);
                score -= overLaps.GroupBy(slot => slot.Place).Sum(x => x.Count() - 1);
                score -= overLaps.GroupBy(slot => slot.Course).Sum(x => x.Count() - 1);
                score -= overLaps.Sum(item => Convert.ToInt32(item.Group == value.Group));
            }

            score -= values.GroupBy(v => v.Day).Count() * 0.5;
            return Math.Pow(Math.Abs(score), -1);
        }
    }
}
