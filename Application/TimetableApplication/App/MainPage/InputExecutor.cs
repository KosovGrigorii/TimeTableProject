using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace TimetableApplication
{
    public class InputExecutor
    {
        private readonly SlotInfoDbConverter slotConverter;
        private readonly TimeDurationDbConverter durationConverter;
        private readonly TimespanDbConverter timespanConverter;
        private readonly IDatabaseWrapper<string, DatabaseSlot> slotWrapper;
        private readonly IDatabaseWrapper<string, DatabaseTimeSchedule> timeScheduleWrapper;
        private readonly IDatabaseWrapper<string, DatabaseLessonMinutesDuration> durationWrapper;
        
        public InputExecutor(SlotInfoDbConverter slotConverter,
            TimeDurationDbConverter durationConverter,
            TimespanDbConverter timespanConverter,
            IDatabaseWrapper<string, DatabaseSlot> slotWrapper,
            IDatabaseWrapper<string, DatabaseTimeSchedule> timeScheduleWrapper,
            IDatabaseWrapper<string, DatabaseLessonMinutesDuration> durationWrapper)
        {
            this.slotConverter = slotConverter;
            this.durationConverter = durationConverter;
            this.timespanConverter = timespanConverter;
            this.slotWrapper = slotWrapper;
            this.timeScheduleWrapper = timeScheduleWrapper;
            this.durationWrapper = durationWrapper;
        }
        
        public void SaveInput(User user, IEnumerable<SlotInfo> slotInput, Times timeSchedule)
        {
            slotWrapper.AddRange(user.Id, slotInput.Select(slot => slotConverter.SlotToDatabaseClass(slot, user.Id)));
            durationWrapper.AddRange(user.Id, new []{durationConverter.MinutesDurationToDbClass(timeSchedule.Duration, user.Id)});
            timeScheduleWrapper.AddRange(user.Id, 
                timeSchedule.LessonStarts.Select(x => timespanConverter.TimeSpanToDbClass(x, user.Id)));
        }
    }
}