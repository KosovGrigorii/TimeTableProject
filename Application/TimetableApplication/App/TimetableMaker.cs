using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class TimetableMaker
    {
        public IEnumerable<string> Algoorithms { get; }
        private readonly ConverterToAlgoritmInput algoInputConverter;
        private readonly AlgorithmChooser chooser;
        private readonly SlotInfoDbConverter slotConverter;
        private readonly TimeDurationDbConverter durationConverter;
        private readonly TimespanDbConverter timespanConverter;
        private readonly TimeslotDbConverter timeslotConverter;
        private readonly IDatabaseWrapper<string, DatabaseSlot> slotWrapper;
        private readonly IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper;
        private readonly IDatabaseWrapper<string, DatabaseTimeSchedule> timeScheduleWrapper;
        private readonly IDatabaseWrapper<string, DatabaseLessonMinutesDuration> durationWrapper;
        private readonly IncompleteTasksKeys processIds;
        
        public TimetableMaker(ConverterToAlgoritmInput algoInputConverter,
            AlgorithmChooser chooser,
            SlotInfoDbConverter slotConverter,
            TimeDurationDbConverter durationConverter,
            TimespanDbConverter timespanConverter,
            TimeslotDbConverter timeslotConverter,
            IDatabaseWrapper<string, DatabaseSlot> slotWrapper,
            IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper,
            IDatabaseWrapper<string, DatabaseTimeSchedule> timeScheduleWrapper,
            IDatabaseWrapper<string, DatabaseLessonMinutesDuration> durationWrapper,
            IEnumerable<ITimetableMaker> algorithms,
            IncompleteTasksKeys processIds)
        {
            Algoorithms = algorithms.Select(a => a.Algorithm.Name);
            this.algoInputConverter = algoInputConverter;
            this.chooser = chooser;
            this.slotConverter = slotConverter;
            this.durationConverter = durationConverter;
            this.timespanConverter = timespanConverter;
            this.timeslotConverter = timeslotConverter;
            this.slotWrapper = slotWrapper;
            this.timeslotWrapper = timeslotWrapper;
            this.timeScheduleWrapper = timeScheduleWrapper;
            this.durationWrapper = durationWrapper;
            this.processIds = processIds;
        }
        
        public void AddUserWaitingForTimetable(User user)
            => processIds.UserIds.Add(user.Id);
        
        public void MakeTimetable(User user, string algorithmName, IEnumerable<Filter> filters)
        {
            var lessonStarts = timeScheduleWrapper.ReadBy(user.Id)
                .Select(x => timespanConverter.DbClassToTimeSpan(x)).ToArray();
            if (!lessonStarts.Any())
                lessonStarts = new TimeSpan[] { 
                    new (9, 0, 0),
                    new (10, 40, 0)};
            var courses = slotWrapper.ReadBy(user.Id).Select(x => slotConverter.DbSlotToSlot(x));
            var lessonDuration = durationConverter.DbDurationToInt(durationWrapper.ReadBy(user.Id).First());
            
            var algoInput = algoInputConverter.Convert(courses, filters, lessonStarts, lessonDuration);
            var algorithm = chooser.ChooseAlgorithm(algorithmName);
            var timeslots = algorithm.GetTimetable(algoInput);
            timeslotWrapper.AddRange(user.Id, timeslots.Select(t => timeslotConverter.TimeslotToDatabaseClass(t, user.Id)));
            processIds.UserIds.Remove(user.Id);
        }
    }
}