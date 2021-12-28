using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseLessonMinutesDuration : DatabaseEntity<string>
    {
        public string DurationInMinutes { get; set; }
    }
}