using System;
using TimetableDomain;

namespace TimetableApplication
{
    public class TimeslotDbConverter
    {
        public DatabaseTimeslot TimeslotToDatabaseClass(TimeSlot timeSlot, string uid)
        {
            return new DatabaseTimeslot()
            {
                Id = Guid.NewGuid().ToString(),
                Day = (int)timeSlot.Day,
                Start = (int)timeSlot.Start.TotalMinutes,
                End = (int)timeSlot.End.TotalMinutes,
                Place = timeSlot.Place,
                Course = timeSlot.Course,
                Teacher = timeSlot.Teacher,
                Group = timeSlot.Group,
                KeyId = uid
            };
        }
        
        public TimeSlot DbTimeslotToTimeslot(DatabaseTimeslot dbTimeslot)
        {
            return new TimeSlot()
            {
                Course = dbTimeslot.Course,
                Day = (DayOfWeek) dbTimeslot.Day,
                End = TimeSpan.FromMinutes(dbTimeslot.End),
                Group = dbTimeslot.Group,
                Place = dbTimeslot.Place,
                Start = TimeSpan.FromMinutes(dbTimeslot.Start),
                Teacher = dbTimeslot.Teacher
            };
        }
    }
}