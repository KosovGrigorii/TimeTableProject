using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class TimetableTaskLauncher
    {
        private readonly ConverterToAlgorithmInput algoInputConverter;
        private readonly SlotInfoDbConverter slotConverter;
        private readonly TimeDurationDbConverter durationConverter;
        private readonly TimespanDbConverter timespanConverter;
        private readonly TimeslotDbConverter timeslotConverter;
        private readonly IDatabaseWrapper<string, DatabaseSlot> slotWrapper;
        private readonly IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper;
        private readonly IDatabaseWrapper<string, DatabaseTimeSchedule> timeScheduleWrapper;
        private readonly IDatabaseWrapper<string, DatabaseLessonMinutesDuration> durationWrapper;
        private readonly DependenciesDictionary<AlgoritmInput, IEnumerable<TimeSlot>, IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>> algorithmsDict;

        public TimetableTaskLauncher(ConverterToAlgorithmInput algoInputConverter,
            SlotInfoDbConverter slotConverter,
            TimeDurationDbConverter durationConverter,
            TimespanDbConverter timespanConverter,
            TimeslotDbConverter timeslotConverter,
            IDatabaseWrapper<string, DatabaseSlot> slotWrapper,
            IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper,
            IDatabaseWrapper<string, DatabaseTimeSchedule> timeScheduleWrapper,
            IDatabaseWrapper<string, DatabaseLessonMinutesDuration> durationWrapper,
        DependenciesDictionary<AlgoritmInput, IEnumerable<TimeSlot>, IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>> algorithmsDict)
        {
            this.algoInputConverter = algoInputConverter;
            this.slotConverter = slotConverter;
            this.durationConverter = durationConverter;
            this.timespanConverter = timespanConverter;
            this.timeslotConverter = timeslotConverter;
            this.slotWrapper = slotWrapper;
            this.timeslotWrapper = timeslotWrapper;
            this.timeScheduleWrapper = timeScheduleWrapper;
            this.durationWrapper = durationWrapper;
            this.algorithmsDict = algorithmsDict;
        }
        
        public Task MakeTimetable(User user, string algorithmName, IEnumerable<Filter> filters)
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
            var timetableTask = new Task(() =>
            {
                Thread.Sleep(5000);
                var timeslots = algorithmsDict.GetResult(algorithmName, algoInput);
                timeslotWrapper.AddRange(user.Id,
                    timeslots.Select(t => timeslotConverter.TimeslotToDatabaseClass(t, user.Id)));
            });
            timetableTask.Start();
            return timetableTask;
        }
    }
}