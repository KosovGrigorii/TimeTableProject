using System;
using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseSlot : DatabaseEntity<string>
    {
        public string Course { get; set; }
        public string Group { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }

        public DatabaseSlot() { }

        public DatabaseSlot(SlotInfo slot, string uid)
        {
            Id = Guid.NewGuid().ToString();
            Course = slot.Course;
            Group = slot.Group;
            Teacher = slot.Teacher;
            Room = slot.Room;
            KeyId = uid;
        }

        public SlotInfo ConvertToSlotInfo()
        {
            return new SlotInfo()
            {
                Course = Course,
                Group = Group,
                Teacher = Teacher,
                Room = Room
            };
        }
    }
}