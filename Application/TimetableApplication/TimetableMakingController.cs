using System;
using System.Collections.Generic;
using TimetableDomain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using TimeSlot = TimetableDomain.TimeSlot;


namespace TimetableApplication
{
    public class TimetableMakingController
    {
        private IEnumerable<Entity> data;
        private DataContext dbData;// вопрос расширяемости(надо использовать DbContext)
        private List<TimeSlot> timeslots;

        public void GetEntitiesFromDB()
        {
            //dbData = DBShell.GetAll();
            
            var converter = new DbToAlgoContentConverter();
            //data = converter.Convert(dbData); этот конвертер нужно реализовать
        }

        public void StartMalingTimeTable()
        {
            var algoithm = new GeneticAlgorithm();
            timeslots = algoithm.Start(dbData.Courses, dbData.Classes); // передать data
                //? алгоритм возвращает список таймслотов
            dbData.TimeSlots.AddRange(timeslots);// нужен конвертер сущности таймслота в доменах в сущность базы
            dbData.SaveChanges();
        }

        public void GetResult() 
        {
            // выгружает файл во временное хранилище
            throw new NotImplementedException();
        }
    }
}