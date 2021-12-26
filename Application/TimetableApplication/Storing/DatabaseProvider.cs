using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class DatabaseProvider
    {
        public int LessonDuration { get; private set; }
        private TimetableDatabases databases;
        private DatabaseEntityConverter converter;

        public DatabaseProvider(DatabasesChooser chooser, DatabaseEntityConverter converter)
        {
            databases = chooser.GetDatabaseWrappers();
            this.converter = converter;
        }

        public void AddInputSlotInfo(string uid, IEnumerable<SlotInfo> inputData)
        => databases.SlotWrapper.AddRange(uid, inputData.Select(slot => converter.SlotToDatabaseClass(slot, uid)));

        public void AddTimeSchedule(string uid, Times timeSchedule)
        {
            LessonDuration = timeSchedule.Duration;
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

        public void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        => databases.TimeslotWrapper.AddRange(uid, timeslots.Select(t => converter.TimeslotToDatabaseClass(t, uid)));

        public IEnumerable<TimeSlot> GetTimeslots(string uid)
            => databases.TimeslotWrapper.ReadBy(uid).Select(x => converter.DbTimeslotToTimeslot(x));

        public void DeleteUserData(string uid)
        {
            databases.SlotWrapper.DeleteKey(uid);
            databases.TimeslotWrapper.DeleteKey(uid);
        }
    }
}