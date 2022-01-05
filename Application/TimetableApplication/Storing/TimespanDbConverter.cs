using System;

namespace TimetableApplication
{
    public class TimespanDbConverter
    {
        public DatabaseTimeSchedule TimeSpanToDbClass(TimeSpan time, string uid)
        {
            return new DatabaseTimeSchedule()
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = time.ToString(),
                KeyId = uid
            };
        }
        
        public TimeSpan DbClassToTimeSpan(DatabaseTimeSchedule time)
            => TimeSpan.Parse(time.StartTime);
    }
}