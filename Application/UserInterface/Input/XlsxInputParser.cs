using System;
using System.Collections.Generic;
using ExcelDataReader;
using System.Linq;
using Infrastructure;
using TimetableApplication;
using System.IO;

namespace UserInterface
{
    public class XlsxInputParser : IImplementation<Stream, UserInput>
    {
        public string Name => "xlsx";
        
        public UserInput GetResult(Stream parameters)
        {
            return ParseFile(parameters);
        }

        private UserInput ParseFile(Stream stream)
        {
            var slots = new List<SlotInfo>();
            var times = new Times() { LessonStarts = new List<TimeSpan>() };
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            while (reader.Read())
            {
                if (reader.GetValue(0) != null && reader.GetValue(0).ToString() == "-")
                {
                    times = GetTimes(reader);
                    break;
                }
                if (reader.GetValue(0) == null || reader.GetValue(1) == null || reader.GetValue(2) == null || reader.GetValue(3) == null)
                {
                    slots.Add(null);
                    break;
                }
                slots.Add(new SlotInfo
                {
                    Course = reader.GetValue(0).ToString(),
                    Group = reader.GetValue(1).ToString(),
                    Teacher = reader.GetValue(2).ToString(),
                    Room = reader.GetValue(3).ToString()

                });
            }
            reader.Close();
            return new () {CourseSlots = slots, TimeSchedule = times};
        }

        private Times GetTimes(IExcelDataReader reader)
        {
            reader.Read();
            if (!TimelineIsCorrectly(reader)) return new Times();
            var (begin, end) = GetSpan(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
            var (duration, rest) = (int.Parse(reader.GetValue(2).ToString()), int.Parse(reader.GetValue(3).ToString()));
            var special_rest = new List<double>();
            var times = new Times() { Duration = duration, LessonStarts = new List<TimeSpan>() };
            var temp = 4;
            while (true)
            {
                try
                {
                    reader.GetValue(temp);
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
                if (reader.GetValue(temp) == null) break;
                var (start_rest, end_rest) = GetSpan(reader.GetValue(temp).ToString(), reader.GetValue(temp + 1).ToString());
                special_rest.Add(start_rest);
                special_rest.Add(end_rest);
                temp += 2;
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

        private bool TimelineIsCorrectly(IExcelDataReader reader)
        {
            var info = new List<string>();
            int temp = 0;
            while (true)
            {
                try
                {
                    reader.GetValue(temp);
                    if (reader.GetValue(temp) == null) break;
                }
                catch(IndexOutOfRangeException)
                {
                    break;
                }
                info.Add(reader.GetValue(temp).ToString().Split().Last());
                temp += 1;
            }
            if (info.Count < 4 || (info.Count - 4) % 2 == 1) return false;
            try
            {
                TimeSpan.Parse(info[0]);
                if (int.TryParse(info[0], out temp)) return false;
                TimeSpan.Parse(info[1]);
                if (int.TryParse(info[1], out temp)) return false;
                int.Parse(info[2]);
                int.Parse(info[3]);
                for (var i = 4; i < info.Count; i++)
                {
                    TimeSpan.Parse(info[i]);
                    if (int.TryParse(info[i], out temp)) return false;
                }
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }
    }
}