using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class App
    {
        private readonly DatabaseProvider databaseProvider;
        private readonly ConverterToAlgoritmInput converter;
        private readonly ITimetableMaker algorithm;
        private readonly OutputProvider outputProvider;
        
        public App(DatabaseProvider databaseProvider, 
            ConverterToAlgoritmInput converter,
            ITimetableMaker algorithm,
            OutputProvider outputProvider)
        {
            this.databaseProvider = databaseProvider;
            this.converter = converter;
            this.algorithm = algorithm;
            this.outputProvider = outputProvider;
        }

        public void SaveInput(string uid, IEnumerable<SlotInfo> slotInput, Times TimeSchedule)
        {
            databaseProvider.AddInputSlotInfo(uid, slotInput);
            databaseProvider.AddTimeSchedule(uid, TimeSchedule);
        }

        public IEnumerable<string> GetTeachers(string uid)
        => databaseProvider.GetTeacherFilters(uid);

        public void MakeTimetable(string uid, IEnumerable<Filter> filters)
        {
            var lessonStarts = databaseProvider.GetTimeSchedule(uid);
            if (!lessonStarts.Any())
                lessonStarts = new List<TimeSpan>() { 
                    new TimeSpan(9, 0, 0),
                    new TimeSpan(10, 40, 0)};
            var courses = databaseProvider.GetInputInfo(uid);
            var lessonDuration = databaseProvider.GetLessonDuration(uid);
            
            var algoInput = converter.Convert(courses, filters, lessonStarts, lessonDuration);
            
            var timeslots = algorithm.GetTimetable(algoInput);
            databaseProvider.SetTimeslots(uid, timeslots);
        }

        public byte[] GetOutput(string uid, OutputExtension extension)
        {
            var timeslots = databaseProvider.GetTimeslots(uid);
            return outputProvider.GetOutputFileStream(extension, uid, timeslots);
        }
    }
}