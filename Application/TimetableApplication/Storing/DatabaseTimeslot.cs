using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseTimeslot : DatabaseEntity<string>
    {
        public int Day { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Place { get; set; }
        public string Course { get; set; }
        public string Teacher { get; set; }
        public string Group { get; set; }
    }
}