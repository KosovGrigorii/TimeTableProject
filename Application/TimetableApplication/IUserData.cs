using System.Collections.Generic;
using TimetableDomain;

namespace TimetableApplication
{
    public interface IUserData
    {
        void AddUser(string uid);
        void SetInputInfo(string uid, IEnumerable<SlotInfo> slots);
        IEnumerable<SlotInfo> GetInputInfo(string uid);
        IEnumerable<string> GetTeacherFilters(string uid);
        void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots);
        IEnumerable<TimeSlot> GetTimeslots(string uid);
    }
}