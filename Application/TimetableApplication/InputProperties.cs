using System.Collections.Generic;

namespace TimetableApplication
{
    public class InputProperties
    {
        public class Course
        {
            public string Title { get; set; }
            public Teacher Teacher { get; set; }
            public List<Group> Groups { get; set; }
        }
        
        public class Class
        {
            public int RoomNumber { get; set; }
        }
        
        public class Group
        {
            public string GroupNumber { get; set; }
        }

        public class Teacher
        {
            public string Name { get; set; }
        }
    }
}