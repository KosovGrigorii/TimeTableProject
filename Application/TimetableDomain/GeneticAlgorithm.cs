using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;

namespace TimetableDomain
{
    public class GeneticAlgorithm : ITimetableMaker
    {
        public Algorithm Name => Algorithm.Genetic;
        
        public IEnumerable<TimeSlot> GetTimetable(
            IEnumerable<Course> cources, 
            IEnumerable<string> classes, 
            IEnumerable<Teacher> teachers, 
            IEnumerable<TimeSpan> lessonStarts)
        {
            Population population = new Population(1000, new TimeTableChromosome(cources, classes, lessonStarts),
                new FitnessFunction(teachers), new EliteSelection());

            for(var i = 1; ; i++)
            {
                population.RunEpoch();
                if (population.FitnessMax >= 0.99 || i >= 1000)
                {
                    break;
                }
            }

            var timetable = (population.BestChromosome as TimeTableChromosome).Value.Select(chromosome =>
                new TimeSlot()
                    {
                        Course = chromosome.Course,
                        Day = (DayOfWeek) chromosome.Day,
                        End = chromosome.EndAt,
                        Group = chromosome.Group,
                        Place = chromosome.Place,
                        Start = chromosome.StartAt,
                        Teacher = chromosome.Teacher
                    });
            return timetable;
        }
    }
}