using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;

namespace TimetableDomain
{
    public class GeneticAlgorithm : ITimetableMaker
    {
        public TimetableMakers Name => TimetableMakers.Genetic;
        
        public IEnumerable<TimeSlot> Start(IEnumerable<Course> cources, 
            IEnumerable<string> classes, IEnumerable<Teacher> teachers, IEnumerable<TimeSpan> lessonStarts)
        {
            Population population = new Population(1000, new TimeTableChromosome(cources, classes, lessonStarts),
                new FitnessFunction(teachers), new EliteSelection());

            int i = 0;
            while (true)
            {
                population.RunEpoch();
                i++;
                if (population.FitnessMax >= 0.99 || i >= 1000)
                {
                    break;
                }
            }

            var timetable = (population.BestChromosome as TimeTableChromosome).Value.Select(chromosome =>
                new TimeSlot(
                    (DayOfWeek) chromosome.Day,
                    chromosome.StartAt, 
                    chromosome.EndAt, 
                    chromosome.PlaceId, 
                    chromosome.CourseId, 
                    chromosome.TeacherId, 
                    chromosome.Groups));
            return timetable;
        }
    }
}