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
                .Where(slot => slot.Day == current.Day && (slot.StartAt == current.StartAt
                                                           || slot.StartAt <= current.EndAt 
                                                           && slot.StartAt >= current.StartAt 
                                                           || slot.EndAt >= current.StartAt 
                                                           && slot.EndAt <= current.EndAt)));
            var teacherWorkDaysOverLaps = new Func<TimeSlotChromosome, IEnumerable<TimeSlotChromosome>>(current =>
                values.Where(x => 
                        Teachers.TryGetValue(x.TeacherId, out var teacher) 
                                  && teacher.IsDayForbidden(x.Day)));
            var GetoverLaps = new Func<TimeSlotChromosome, List<TimeSlotChromosome>>(current => 
                getTimeOverLaps(current)
                    .Concat(teacherWorkDaysOverLaps(current))
                    .ToList()
                );

            foreach (var value in values)
            {
                var overLaps = GetoverLaps(value);
                score -= overLaps.GroupBy(slot => slot.TeacherId).Sum(x => x.Count() - 1);
                score -= overLaps.GroupBy(slot => slot.PlaceId).Sum(x => x.Count() - 1);
                score -= overLaps.GroupBy(slot => slot.CourseId).Sum(x => x.Count() - 1);
                score -= overLaps.Sum(item => item.Groups.Intersect(value.Groups).Count());
                //score -= overLaps.Sum(item => item.Group.Intersect(value.Groups).Count());
            }

            score -= values.GroupBy(v => v.Day).Count() * 0.5;
            return Math.Pow(Math.Abs(score), -1);
        }
    }
}
