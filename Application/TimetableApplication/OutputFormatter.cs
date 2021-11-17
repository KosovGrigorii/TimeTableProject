using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public abstract class OutputFormatter
    {
        public abstract FileInfo GetOutputFile(IEnumerable<TimeSlot> timeSlots);
        
        protected (Dictionary<string, string[,]>, List<Tuple<TimeSpan, TimeSpan>>) ConvertTimeSlotsToDictionaries(
            List<TimeSlot> timeSlots)
        {
            var teachers = new Dictionary<Teacher, List<TimeSlot>>();
            var groups = new Dictionary<Group, List<TimeSlot>>();
            var bellSet = new HashSet<Tuple<TimeSpan, TimeSpan>>();
            foreach (var timeSlot in timeSlots)
            {
                bellSet.Add(Tuple.Create(timeSlot.Start, timeSlot.End));
                var teacher = timeSlot.Course.Teacher;
                if (!teachers.ContainsKey(teacher))
                    teachers[teacher] = new List<TimeSlot>();
                teachers[teacher].Add(timeSlot);
                foreach (var group in timeSlot.Groups)
                {
                    if (!groups.ContainsKey(group))
                        groups[group] = new List<TimeSlot>();
                    groups[group].Add(timeSlot);
                }
            }
            
            var bells = bellSet.OrderBy(tuple => tuple.Item1.TotalHours).ToList();
            var teachersSchedule = GetScheduleTable(teachers, bells,
                teacher => teacher.Name,
                slot => string.Join(", ", slot.Groups.Select(group => group.Name)));
            var groupsSchedule = GetScheduleTable(groups, bells,
                group => group.Name,
                slot => slot.Course.Teacher.Name);
            var schedule = teachersSchedule
                .Concat(groupsSchedule)
                .ToDictionary(tuple => tuple.Key, tuple => tuple.Value);
            return (schedule, bells);
        }

        private Dictionary<string, string[,]> GetScheduleTable<T>(Dictionary<T, List<TimeSlot>> dict,
            IList<Tuple<TimeSpan, TimeSpan>> bells, Func<T, string> getCurrentName, Func<TimeSlot, string> getOtherName)
        {
            var result = new Dictionary<string, string[,]>();
            foreach (var (entity, slots) in dict)
            {
                var currentName = getCurrentName(entity);
                result[currentName] = new string[bells.Count, 7];
                foreach (var slot in slots)
                {
                    var start = bells.IndexOf(Tuple.Create(slot.Start, slot.End));
                    var day = (Convert.ToInt32(slot.Day) - 1) % 7;
                    result[currentName][start, day] = slot.ToString(getOtherName(slot));
                }
            }
            return result;
        } 
    }

    public static class TimeSlotExtension
    {
        public static string ToString(this TimeSlot slot, string name)
            => $"{slot.Course.Title}\n{name}\n{slot.Place.Name ?? "nowhere"}";
    }
}
