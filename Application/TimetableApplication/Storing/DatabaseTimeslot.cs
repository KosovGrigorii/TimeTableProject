using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseTimeslot : DatabaseEntity<string>
    {
        public int Day { get; init; }
        public int Start { get; init; }
        public int End { get; init; }
        public string Place { get; init; }
        public string Course { get; init; }
        public string Teacher { get; init; }
        public string Group { get; init; }
    }
}