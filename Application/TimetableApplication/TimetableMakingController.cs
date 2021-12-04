using System;
using System.Collections.Generic;
using System.Linq;
using Accord;
using TimetableDomain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using TimeSlot = TimetableDomain.TimeSlot;


namespace TimetableApplication
{
    public class TimetableMakingController
    {
        private IEnumerable<Entity> data;
        //private DataContext dbData; вопрос расширяемости(надо использовать DbContext)
        private List<TimeSlot> timeslots;

        public void GetEntitiesFromDB() //private?
        {
            var converter = new DbToAlgoContentConverter();
        }

        public void StartMakingTimeTable(IEnumerable<Filter> filters)
        {
            GetEntitiesFromDB(); //Вернуть data, не сохранять в поле
            var algoithm = new GeneticAlgorithm();
            var courses = DB.Slots
                .Select(x => new Course()
                {
                    Id = x.Course,
                    Title = x.Course,
                    Teacher = x.Teacher,
                    Groups = new List<string>() {x.Group}
                }).ToList();
            var classes = DB.Slots.Select(x => new Class() {Id = x.Class}).ToList();
            timeslots = algoithm.Start(courses, classes); // передать data
            DB.Timeslots = timeslots;
            // dbData.TimeSlots.AddRange(timeslots);// нужен конвертер сущности таймслота в доменах в сущность базы
            // dbData.SaveChanges();
        }

        public void GetResult() 
        {
            // выгружает файл во временное хранилище
            throw new NotImplementedException();
        }
    }
}