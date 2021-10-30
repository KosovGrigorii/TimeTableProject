using System;
using System.Collections.Generic;
using TimetableDomain;
using Infrastructure;
using TimeSlot = TimetableDomain.TimeSlot;


namespace TimetableApplication
{
    public class TimetableMakingController
    {
        private IEnumerable<Entity> data;
        private List<TimeSlot> timeslots;

        public void GetEntitiesFromDB()
        {
            var dbData = DBShell.GetAll();
            
            var converter = new DbToAlgoContentConverter();
            data = converter.Convert(dbData);
        }

        public void StartMalingTimeTable()
        {
            var algoithm = new GeneticAlgorithm();
            algoithm.Start(); // передать data
                //? алгоритм возвращает список таймслотов
            timeslots = new List<TimeSlot>();
        }

        public void GetResult() 
        {
            // выгружает файл во временное хранилище
            throw new NotImplementedException();
        }
    }
}