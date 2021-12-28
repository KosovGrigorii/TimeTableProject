using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class DatabaseProvider
    {
        private TimetableDatabases databases;
        private DatabaseEntityConverter converter;

        public DatabaseProvider(TimetableDatabases databases, DatabaseEntityConverter converter)
        {
            this.converter = converter;
            this.databases = databases;
        }

        public void AddInputSlotInfo(string uid, IEnumerable<SlotInfo> inputData)
        => databases.SlotWrapper.AddRange(uid, inputData.Select(slot => converter.SlotToDatabaseClass(slot, uid)));

        public void AddTimeSchedule(string uid, Times timeSchedule)
        {
            databases.DurationWrapper.AddRange(uid, new []{converter.MinutesDurationToDbClass(timeSchedule.Duration, uid)});
            databases.TimeScheduleWrapper.AddRange(uid, 
                timeSchedule.LessonStarts.Select(x => converter.TimeSpanToDbClass(x, uid)));
        }

        public IEnumerable<SlotInfo> GetInputInfo(string uid)
            => databases.SlotWrapper.ReadBy(uid).Select(x => converter.DbSlotToSlot(x));

        public IEnumerable<TimeSpan> GetTimeSchedule(string uid)
            => databases.TimeScheduleWrapper.ReadBy(uid)
                .Select(x => converter.DbClassToTimeSpan(x));

        public IEnumerable<string> GetTeacherFilters(string uid)
        {
            return databases.SlotWrapper
                .ReadBy(uid)
                .Select(x => x.Teacher)
                .Distinct();
        }

        public int GetLessonDuration(string uid)
            => converter.DbDurationToInt(databases.DurationWrapper.ReadBy(uid).First());

        public void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        => databases.TimeslotWrapper.AddRange(uid, timeslots.Select(t => converter.TimeslotToDatabaseClass(t, uid)));

        public IEnumerable<TimeSlot> GetTimeslots(string uid)
            => databases.TimeslotWrapper.ReadBy(uid).Select(x => converter.DbTimeslotToTimeslot(x));
    }
}