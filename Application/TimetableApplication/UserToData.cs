using System;
using System.Collections.Generic;
using TimetableCommonClasses;

namespace TimetableApplication
{
    public static class UserToData
    {
        private class UserData
        {
            internal IEnumerable<SlotInfo> Slots { get; set; }
            internal IEnumerable<TimeSlot> Timeslots { get; set; }
        }
        
        private static readonly Dictionary<string, UserData> uidToData = new();

        public static void AddUser(string uid)
        {
            if (!uidToData.ContainsKey(uid))
                uidToData[uid] = new UserData();
            else
                throw new ArgumentException("User with this id is already added");
        }

        public static void SetInputInfo(string uid, IEnumerable<SlotInfo> slots)
        {
            if (uidToData.TryGetValue(uid, out var data))
                data.Slots = slots;
            else
                throw new ArgumentException("No such UID");
        }
        
        public static IEnumerable<SlotInfo> GetInputInfo(string uid)
        {
            if (uidToData.TryGetValue(uid, out var data))
                return data.Slots;
            throw new ArgumentException("No such UID");
        }

        public static void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        {
            if (uidToData.TryGetValue(uid, out var data))
                data.Timeslots = timeslots;
            else
                throw new ArgumentException("No such UID");
        }
        
        public static IEnumerable<TimeSlot> GetTimeslots(string uid)
        {
            if (uidToData.TryGetValue(uid, out var data))
                return data.Timeslots;
            throw new ArgumentException("No such UID");
        }
    }
}