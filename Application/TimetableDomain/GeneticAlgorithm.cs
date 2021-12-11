using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accord.Genetic;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TimetableDomain
{
    public class GeneticAlgorithm : ITimetableMaker
    {
        public string Name { get; }

        public GeneticAlgorithm()
        {
            Name = "Genetic";
        }
        
        public List<TimeSlot> Start(List<Course> cources, List<string> classes)  //(string[] args) Demo чтобы не было ошибок, а так это потенциальный main для алгоритмов
        {
            Population population = new Population(1000, new TimeTableChromosome(cources, classes),
                new TimeTableChromosome.FitnessFunction(), new EliteSelection());

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
                new TimeSlot()
                {
                    Course = chromosome.CourseId, 
                    Place = chromosome.PlaceId,
                    Teacher = chromosome.TeacherId,
                    Day = (DayOfWeek) chromosome.Day,
                    Start = chromosome.StartAt, End = chromosome.EndAt,
                    Id = Guid.NewGuid().ToString(),
                    Groups = chromosome.Groups
                }
            ).ToList();
            return timetable;
        }
    }
}