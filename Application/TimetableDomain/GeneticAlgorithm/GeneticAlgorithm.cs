using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;

namespace TimetableDomain
{
    public class GeneticAlgorithm : ITimetableMaker
    {
        public Algorithm Name => Algorithm.Genetic;
        private EliteSelection eliteSelection;
        private FitnessFunction fitnessFunction;

        public GeneticAlgorithm(EliteSelection eliteSelection, FitnessFunction fitnessFunction)
        {
            this.eliteSelection = eliteSelection;
            this.fitnessFunction = fitnessFunction;
        }
        
        public IEnumerable<TimeSlot> GetTimetable(
            IEnumerable<Course> cources, 
            IEnumerable<Teacher> teachers, 
            IEnumerable<TimeSpan> lessonStarts)
        {
            fitnessFunction.SetTeachersFilters(teachers);
            
            var population = new Population(1000, new TimeTableChromosome(cources, lessonStarts),
                fitnessFunction, eliteSelection);

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
                        Day = (DayOfWeek) chromosome.Day,
                        Start = chromosome.StartAt,
                        End = chromosome.EndAt,
                        Place = chromosome.Course.Place,
                        Course = chromosome.Course.Title,
                        Teacher = chromosome.Teacher,
                        Group =  chromosome.Group
                    });
            return timetable;
        }
    }
}