using System.Collections.Generic;
using TimetableDomain;

namespace TimetableApplication
{
    public class UserInputData
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public SlotInfo Slot { get; set; }
    }
    
    public class UserTimeslots
    {
        public int Id { get; set; }
            public string UserID { get; set; }
            public TimeSlot Timeslot { get; set; }
    }
}