using System;

namespace TimetableApplication
{
    public class TimeDurationDbConverter
    {
        public DatabaseLessonMinutesDuration MinutesDurationToDbClass(int time, string uid)
        {
            return new DatabaseLessonMinutesDuration()
            {
                Id = Guid.NewGuid().ToString(),
                DurationInMinutes = time.ToString(),
                KeyId = uid
            };
        }

        public int DbDurationToInt(DatabaseLessonMinutesDuration duration)
            => int.Parse(duration.DurationInMinutes);
    }
}