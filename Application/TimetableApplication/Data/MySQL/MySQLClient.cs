using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class MySQLClient : IDatabaseClient
    {
        public DatabaseClient Name => DatabaseClient.MySQL;
        private readonly MySQLDataContext dataContext;
        
        public MySQLClient(MySQLDataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        
        public void SetInputInfo(string uid, IEnumerable<SlotInfo> slots)
        {
            var dbSlots = slots.Select(x => new DBSlotInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Course = x.Course,
                Group = x.Group,
                Room = x.Room,
                Teacher = x.Teacher
            });
            var user = new User()
            {
                Id = uid,
                InputData = dbSlots.ToList(),
            };
            dataContext.Users.Add(user);
            dataContext.SaveChanges();
        }

        public IEnumerable<SlotInfo> GetInputInfo(string uid)
        {
            return dataContext.Users
                .Find(uid)
                .InputData
                .Select(dbslot => new SlotInfo()
                {
                    Room = dbslot.Room,
                    Course = dbslot.Course,
                    Group = dbslot.Group,
                    Teacher = dbslot.Teacher
                });
        }

        public IEnumerable<string> GetTeacherFilters(string uid)
        {
            return dataContext.Users
                .Find(uid)
                .InputData
                .Select(x => x.Teacher)
                .Distinct();
        }

        public void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        {
            var dbEntities = timeslots.Select(x =>  new DBTimeSlot()
            {
                Id = Guid.NewGuid().ToString(),
                Course = x.Course,
                Day = (int)x.Day,
                End = x.End.Minutes,
                Group = x.Group,
                Place = x.Place,
                Start = x.Start.Minutes,
                Teacher = x.Teacher
            });
            dataContext.Users.Find(uid).TimeSlots = dbEntities.ToList();
            dataContext.SaveChanges();
        }

        public IEnumerable<TimeSlot> GetTimeslots(string uid)
        {
            return dataContext.Users.Find(uid).TimeSlots.Select(x => new TimeSlot()
            {
                Course = x.Course,
                Day = (DayOfWeek)x.Day,
                End = TimeSpan.FromMinutes(x.End),
                Group = x.Group,
                Place = x.Place,
                Start = TimeSpan.FromMinutes(x.Start),
                Teacher = x.Teacher
            });
        }
        
        public void DeleteUserData(string uid)
        {
            dataContext.Users.Remove(dataContext.Users.Find(uid));
            dataContext.SaveChanges();
        }
    }
}