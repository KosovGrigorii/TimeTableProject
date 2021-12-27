using Infrastructure;

namespace TimetableApplication
{
    public class TimetableDatabases
    {
        public IDatabaseWrapper<string, DatabaseSlot> SlotWrapper { get; }
        public IDatabaseWrapper<string, DatabaseTimeslot> TimeslotWrapper { get; }
        public IDatabaseWrapper<string, DatabaseTimeSchedule> TimeScheduleWrapper { get; }

        public TimetableDatabases(
            IDatabaseWrapper<string, DatabaseSlot> slotWrapper,
            IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper,
            IDatabaseWrapper<string, DatabaseTimeSchedule> timeScheduleWrapper)
        {
            SlotWrapper = slotWrapper;
            TimeslotWrapper = timeslotWrapper;
            TimeScheduleWrapper = timeScheduleWrapper;
        }
    }
}