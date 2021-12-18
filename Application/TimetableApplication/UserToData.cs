using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class UserToData : IUserData
    {
        private class UserData
        {
            internal IEnumerable<SlotInfo> Slots { get; set; }
            internal IEnumerable<TimeSlot> Timeslots { get; set; }
        }
        
        private readonly Dictionary<string, UserData> uidToData = new();

        public void AddUser(string uid)
        {
            if (!uidToData.ContainsKey(uid))
                uidToData[uid] = new UserData();
            else
                throw new ArgumentException("User with this id is already added");
        }

        public void SetInputInfo(string uid, IEnumerable<SlotInfo> slots)
        {
            if (uidToData.TryGetValue(uid, out var data))
                data.Slots = slots;
            else
                throw new ArgumentException("No such UID");
        }
        
        public IEnumerable<SlotInfo> GetInputInfo(string uid)
        {
            if (uidToData.TryGetValue(uid, out var data))
                return data.Slots;
            throw new ArgumentException("No such UID");
        }
        
        public IEnumerable<string> GetTeacherFilters(string uid)
        {
            if (uidToData.TryGetValue(uid, out var data))
                return data.Slots.Select(x => x.Teacher).Distinct();
            throw new ArgumentException("No such UID");
        }

        public void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        {
            if (uidToData.TryGetValue(uid, out var data))
                data.Timeslots = timeslots;
            else
                throw new ArgumentException("No such UID");
        }
        
        public IEnumerable<TimeSlot> GetTimeslots(string uid)
        {
            if (uidToData.TryGetValue(uid, out var data))
                return data.Timeslots;
            throw new ArgumentException("No such UID");
        }
    }
}