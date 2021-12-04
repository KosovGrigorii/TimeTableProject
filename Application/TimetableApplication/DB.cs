using System.Collections.Generic;
using TimetableDomain;

namespace TimetableApplication
{
    public class DB
    {
        public static HashSet<SlotInfo> Slots { get; set; }
        public static List<TimeSlot> Timeslots { get; set; }
    }
}