using System.Collections.Generic;
using TimetableApplication;

namespace UserInterface
{
    public class UserInput
    {
        public IEnumerable<SlotInfo> CourseSlots { get; init; }
        public Times TimeSchedule { get; init; }
    }
}