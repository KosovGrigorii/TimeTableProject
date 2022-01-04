using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;

namespace TimetableDomain
{
    public class GeneticAlgorithm : ITimetableMaker
    {
        public Algorithm Algorithm { get; }
        private readonly EliteSelection eliteSelection;
        private readonly FitnessFunction fitnessFunction;

        public GeneticAlgorithm(EliteSelection eliteSelection, FitnessFunction fitnessFunction)
        {
            this.eliteSelection = eliteSelection;
            this.fitnessFunction = fitnessFunction;
            Algorithm = new Algorithm("Genetic");
        }
        
        public IEnumerable<TimeSlot> GetTimetable(AlgoritmInput input)
        {
            fitnessFunction.SetTeachersFilters(input.TeacherFilters);
            
            var population = new Population(1000, new TimeTableChromosome(input.Courses, input.LessonStarts),
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