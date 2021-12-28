using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class App
    {
        private readonly DatabaseProvider databaseProvider;
        private readonly ConverterToAlgoritmInput converter;
        private readonly AlgorithmChooser chooser;
        private readonly OutputProvider outputProvider;
        
        public App(DatabaseProvider databaseProvider, 
            ConverterToAlgoritmInput converter,
            OutputProvider outputProvider,
            AlgorithmChooser chooser)
        {
            this.databaseProvider = databaseProvider;
            this.converter = converter;
            this.outputProvider = outputProvider;
            this.chooser = chooser;
        }

        public void SaveInput(string uid, IEnumerable<SlotInfo> slotInput, Times TimeSchedule)
        {
            databaseProvider.AddInputSlotInfo(uid, slotInput);
            databaseProvider.AddTimeSchedule(uid, TimeSchedule);
        }
        
        public Array GetAlgorithmNames()
        => Enum.GetValues(typeof(Algorithm));
        public IEnumerable<string> GetTeachers(string uid)
        => databaseProvider.GetTeacherFilters(uid);

        public void MakeTimetable(string uid, Algorithm algorithmName, IEnumerable<Filter> filters)
        {
            var lessonStarts = databaseProvider.GetTimeSchedule(uid);
            if (!lessonStarts.Any())
                lessonStarts = new List<TimeSpan>() { 
                    new TimeSpan(9, 0, 0),
                    new TimeSpan(10, 40, 0)};
            var courses = databaseProvider.GetInputInfo(uid);
            var lessonDuration = databaseProvider.GetLessonDuration(uid);
            
            var algoInput = converter.Convert(courses, filters, lessonStarts, lessonDuration);
            var algorithm = chooser.ChooseAlgorithm(algorithmName);
            var timeslots = algorithm.GetTimetable(algoInput);
            databaseProvider.SetTimeslots(uid, timeslots);
        }

        public Array GetOutputExtensions()
            => Enum.GetValues(typeof(OutputExtension));

        public byte[] GetOutput(string uid, OutputExtension extension)
        {
            var timeslots = databaseProvider.GetTimeslots(uid);
            return outputProvider.GetOutputFileStream(extension, uid, timeslots);
        }
    }
}