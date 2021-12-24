using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class DatabaseProvider
    {
        private TimetableDatabases databases;

        public DatabaseProvider(DatabasesChooser chooser)
        {
            databases = chooser.GetDatabaseWrappers();
        }

        public void AddInputSlotInfo(string uid, IEnumerable<SlotInfo> inputData)
        => databases.SlotWrapper.AddRange(uid, inputData.Select(slot => new DatabaseSlot(slot, uid)));

        public IEnumerable<SlotInfo> GetInputInfo(string uid)
            => databases.SlotWrapper.ReadBy(uid).Select(x => x.ConvertToSlotInfo());

        public IEnumerable<string> GetTeacherFilters(string uid)
        {
            return databases.SlotWrapper
                .ReadBy(uid)
                .Select(x => x.Teacher)
                .Distinct();
        }

        public void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        => databases.TimeslotWrapper.AddRange(uid, timeslots.Select(t => new DatabaseTimeslot(t, uid)));

        public IEnumerable<TimeSlot> GetTimeslots(string uid)
            => databases.TimeslotWrapper.ReadBy(uid).Select(x => x.ConvertToTimeslot());

        public void DeleteUserData(string uid)
        {
            databases.SlotWrapper.DeleteKey(uid);
            databases.TimeslotWrapper.DeleteKey(uid);
        }
    }
}