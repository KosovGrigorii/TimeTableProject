using System.Collections.Generic;

namespace TimetableApplication
{
    public class User
    {
        public string Id { get; set; }

        public IList<DBSlotInfo> InputData { get; set; }
        
        public IList<DBTimeSlot> TimeSlots { get; set; }
    }
}