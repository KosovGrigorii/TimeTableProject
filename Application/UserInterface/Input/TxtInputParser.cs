using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using TimetableApplication;

namespace UserInterface
{
    class TxtInputParser : IImplementation<Stream, UserInput>
    {
        public string Name => "txt";
        
        public UserInput GetResult(Stream parameters)
        {
            return ParseFile(parameters);
        }

        private UserInput ParseFile(Stream stream)
        {
            var slots = new List<SlotInfo>();
            var times = new Times() { LessonStarts = new List<TimeSpan>() };
            using var reader = new StreamReader(stream);
            var line = reader.ReadLine();
            while (line != null)
            {
                if (line == "-")
                {
                    times = GetTimes(reader);
                    break;
                }
                var slot = line.Split('|');
                try
                {
                    var test = slot[3];
                }
                catch (IndexOutOfRangeException)
                {
                    slots.Add(null);
                    break;
                }
                slots.Add(new()
                {
                    Course = slot[0],
                    Group = slot[1],
                    Teacher = slot[2],
                    Room = slot[3]
                });
                line = reader.ReadLine();
            }
            reader.Close();
            return new() { CourseSlots = slots, TimeSchedule = times };
        }

        private Times GetTimes(StreamReader reader)
        {
            var info = reader.ReadLine().Split();
            var (begin, end) = GetSpan(info[0], info[1]);
            var (duration, rest) = (int.Parse(info[2]), int.Parse(info[3]));
            var special_rest = new List<double>();
            var times = new Times() { Duration = duration, LessonStarts = new List<TimeSpan>() };
            for (var i = 4; i < info.Length; i += 2)
            {
                var (start_rest, end_rest) = GetSpan(info[i], info[i + 1]);
                special_rest.Add(start_rest);
                special_rest.Add(end_rest);
            }
            while (begin < end)
            {
                times.LessonStarts.Add(TimeSpan.FromMinutes(begin));
                if (special_rest.Count == 0 || begin + duration < special_rest[0])
                {
                    begin += duration + rest;
                    continue;
                }
                begin = special_rest[1];
                special_rest.RemoveRange(0, 2);
            }
            return times;
        }

        private (double, double) GetSpan(string begin, string end)
        {
            var temp = begin.Split();
            begin = temp[temp.Length - 1];
            temp = end.Split();
            end = temp[temp.Length - 1];
            temp = begin.Split(':');
            var begin_span = new TimeSpan(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
            temp = end.Split(':');
            var end_span = new TimeSpan(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
            return (begin_span.TotalMinutes, end_span.TotalMinutes);
        }
    }
}