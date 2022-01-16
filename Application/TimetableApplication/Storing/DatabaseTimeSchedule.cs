using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseTimeSchedule : DatabaseEntity<string>
    {
        public string StartTime { get; init; }
    }
}