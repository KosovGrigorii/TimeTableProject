using System;

namespace TimetableApplication
{
    public class SlotInfoDbConverter
    {
        public DatabaseSlot SlotToDatabaseClass(SlotInfo slot, string uid)
        {
            return new DatabaseSlot()
            {
                Id = Guid.NewGuid().ToString(),
                Course = slot.Course,
                Group = slot.Group,
                Teacher = slot.Teacher,
                Room = slot.Room,
                KeyId = uid
            };
        }

        public SlotInfo DbSlotToSlot(DatabaseSlot dbSlot)
        {
            return new SlotInfo()
            {
                Course = dbSlot.Course,
                Group = dbSlot.Group,
                Teacher = dbSlot.Teacher,
                Room = dbSlot.Room
            };
        }
    }
}