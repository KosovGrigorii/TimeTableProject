using System.Collections.Generic;
using TimetableCommonClasses;

namespace TimetableApplication
{
    public class DB
    {
        public static HashSet<SlotInfo> Slots { get; set; }
        public static IEnumerable<TimeSlot> Timeslots { get; set; }
    }
}