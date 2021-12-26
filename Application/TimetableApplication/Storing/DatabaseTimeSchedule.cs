using System;
using Infrastructure;

namespace TimetableApplication
{
    public class DatabaseTimeSchedule : DatabaseEntity<string>
    {
        public string StartTime { get; set; }

        public static DatabaseTimeSchedule TimeSpanToDbClass(TimeSpan time, string uid)
            => new DatabaseTimeSchedule()
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = time.ToString(),
                KeyId = uid
            };
        
        public static TimeSpan DbClassToTimeSpan(DatabaseTimeSchedule time)
        => TimeSpan.Parse(time.StartTime);
    }
}