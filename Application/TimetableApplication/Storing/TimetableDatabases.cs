using Infrastructure;

namespace TimetableApplication
{
    public class TimetableDatabases
    {
        public IDatabaseWrapper<string, DatabaseSlot> SlotWrapper { get; init; }
        public IDatabaseWrapper<string, DatabaseTimeslot> TimeslotWrapper { get; init; }
        public IDatabaseWrapper<string, DatabaseTimeSchedule> TimeScheduleWrapper { get; init; }
    }
}