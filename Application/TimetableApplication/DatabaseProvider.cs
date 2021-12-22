using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class DatabaseProvider 
    {
        private IDatabaseWrapper<string, DatabaseSlot> slotWrapper;
        private IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper;

        public DatabaseProvider(
            IReadOnlyDictionary<Database, IDatabaseWrapper<string, DatabaseSlot>> wrappersSlots,
            IReadOnlyDictionary<Database, IDatabaseWrapper<string, DatabaseTimeslot>> wrappersTimeslots)
        {
            const Database db = Database.MySQL;
            slotWrapper = wrappersSlots[db];
            timeslotWrapper = wrappersTimeslots[db];
        }

        public void AddInputSlotInfo(string uid, IEnumerable<SlotInfo> inputData)
        => slotWrapper.AddRange(uid, inputData.Select(slot => new DatabaseSlot(slot, uid)));

        public IEnumerable<SlotInfo> GetInputInfo(string uid)
            => slotWrapper.ReadBy(uid).Select(x => x.ConvertToSlotInfo());

        public IEnumerable<string> GetTeacherFilters(string uid)
        {
            return slotWrapper
                .ReadBy(uid)
                .Select(x => x.Teacher)
                .Distinct();
        }

        public void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        => timeslotWrapper.AddRange(uid, timeslots.Select(t => new DatabaseTimeslot(t, uid)));

        public IEnumerable<TimeSlot> GetTimeslots(string uid)
            => timeslotWrapper.ReadBy(uid).Select(x => x.ConvertToTimeslot());

        public void DeleteUserData(string uid)
        {
            slotWrapper.DeleteKey(uid);
            timeslotWrapper.DeleteKey(uid);
        }
    }
}