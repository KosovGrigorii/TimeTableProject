using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class OutputConverter
    {
        public Dictionary<string, string[,]> ConvertTimeslotsToDictionary(IEnumerable<TimeSlot> slots)
        {
            var teachers = new Dictionary<string, List<TimeSlot>>();
            var groups = new Dictionary<string, List<TimeSlot>>();
            var bellSet = new HashSet<Tuple<TimeSpan, TimeSpan>>();
            foreach (var timeSlot in slots)
            {
                bellSet.Add(Tuple.Create(timeSlot.Start, timeSlot.End));
                var teacher = timeSlot.Teacher;
                if (!teachers.ContainsKey(teacher))
                    teachers[teacher] = new List<TimeSlot>();
                teachers[teacher].Add(timeSlot);
                var group = timeSlot.Group;
                if (!groups.ContainsKey(group))
                    groups[group] = new List<TimeSlot>();
                groups[group].Add(timeSlot);
            }
            
            var bells = bellSet.OrderBy(tuple => tuple.Item1.TotalHours).ToList(); 
            var teachersTables = teachers
                .Select(pair => Tuple.Create(pair.Key, GetTable(pair.Value, bells, slot => slot.Group)));
            var groupsTables = groups
                .Select(pair => Tuple.Create(pair.Key, GetTable(pair.Value, bells, slot => slot.Teacher)));
            var tables = teachersTables
                .Concat(groupsTables)
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            return tables;
        }

        private string[,] GetTable(IEnumerable<TimeSlot> slots,
            IList<Tuple<TimeSpan, TimeSpan>> bells,
            Func<TimeSlot, string> getOtherName)
        {
            var table = new string[bells.Count + 1, 8];
            for (var i = 0; i < bells.Count; i++)
                table[i + 1, 0] = $"{bells[i].Item1} — {bells[i].Item2}";
            for (var day = 1; day < 8; day++)
                table[0, day] = Enum.GetName(typeof(DayOfWeek), day % 7);
            foreach (var slot in slots)
            {
                var start = bells.IndexOf(Tuple.Create(slot.Start, slot.End));
                var day = (Convert.ToInt32(slot.Day) - 1) % 7;
                table[start + 1, day + 1] = slot.ToString(getOtherName(slot));
            }

            return table;
        }
    }
    
    public static class TimeSlotExtension
    {
        public static string ToString(this TimeSlot slot, string name)
            => $"{slot.Course}\n{name}\n{slot.Place}";
    }
}