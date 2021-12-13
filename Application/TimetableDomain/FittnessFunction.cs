using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;

namespace TimetableDomain
{
    public class FitnessFunction : IFitnessFunction
    {
        public double Evaluate(IChromosome chromosome)
        {
            double score = 1;
            var values = (chromosome as TimeTableChromosome).Value;
            var GetoverLaps = new Func<TimeSlotChromosome, List<TimeSlotChromosome>>(current => values
                .Except(new[] { current })
                .Where(slot => (slot.Day == current.Day && (slot.StartAt == current.StartAt
                                                           || slot.StartAt <= current.EndAt 
                                                           && slot.StartAt >= current.StartAt 
                                                           || slot.EndAt >= current.StartAt 
                                                           && slot.EndAt <= current.EndAt)))
                .ToList());




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
