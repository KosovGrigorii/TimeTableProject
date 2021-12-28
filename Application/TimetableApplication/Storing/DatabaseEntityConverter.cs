using System;
using TimetableDomain;

namespace TimetableApplication
{
    public class DatabaseEntityConverter
    {
        public DatabaseSlot SlotToDatabaseClass(SlotInfo slot, string uid)
        {
            return new DatabaseSlot()
            {
                Id = Guid.NewGuid().ToString(),
                Course = slot.Course,
                Group = slot.Group,
                Teacher = slot.Teacher,
                Room = slot.Room,
                KeyId = uid
            };
        }

        public SlotInfo DbSlotToSlot(DatabaseSlot dbSlot)
        {
            return new SlotInfo()
            {
                Course = dbSlot.Course,
                Group = dbSlot.Group,
                Teacher = dbSlot.Teacher,
                Room = dbSlot.Room
            };
        }

        public DatabaseTimeSchedule TimeSpanToDbClass(TimeSpan time, string uid)
        {
            return new DatabaseTimeSchedule()
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = time.ToString(),
                KeyId = uid
            };
        }
        
        public TimeSpan DbClassToTimeSpan(DatabaseTimeSchedule time)
            => TimeSpan.Parse(time.StartTime);

        public DatabaseLessonMinutesDuration MinutesDurationToDbClass(int time, string uid)
        {
            return new DatabaseLessonMinutesDuration()
            {
                Id = Guid.NewGuid().ToString(),
                DurationInMinutes = time.ToString(),
                KeyId = uid
            };
        }

        public int DbDurationToInt(DatabaseLessonMinutesDuration duration)
            => int.Parse(duration.DurationInMinutes);

        public DatabaseTimeslot TimeslotToDatabaseClass(TimeSlot timeSlot, string uid)
        {
            return new DatabaseTimeslot()
            {
                Id = Guid.NewGuid().ToString(),
                Day = (int)timeSlot.Day,
                Start = (int)timeSlot.Start.TotalMinutes,
                End = (int)timeSlot.End.TotalMinutes,
                Place = timeSlot.Place,
                Course = timeSlot.Course,
                Teacher = timeSlot.Teacher,
                Group = timeSlot.Group,
                KeyId = uid
            };
        }
        
        public TimeSlot DbTimeslotToTimeslot(DatabaseTimeslot dbTimeslot)
        {
            return new TimeSlot()
            {
                Course = dbTimeslot.Course,
                Day = (DayOfWeek) dbTimeslot.Day,
                End = TimeSpan.FromMinutes(dbTimeslot.End),
                Group = dbTimeslot.Group,
                Place = dbTimeslot.Place,
                Start = TimeSpan.FromMinutes(dbTimeslot.Start),
                Teacher = dbTimeslot.Teacher
            };
        }
    }
}