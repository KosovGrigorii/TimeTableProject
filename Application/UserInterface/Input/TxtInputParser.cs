using System;
using System.Collections.Generic;
using System.IO;
using UserInterface;

namespace TimetableApplication
{
    class TxtInputParser : IInputParser
    {
        public (double, double) GetSpan(string begin, string end)
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

        public ParserExtension Extension => ParserExtension.txt;

        public IEnumerable<SlotInfo> ParseFile(Stream stream)
        {
            var slots = new List<SlotInfo>();
            var starts = new List<TimeSpan>();
            var duration = 90;
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            try
            {
                var line = reader.ReadLine();
                while (line != "-")
                {
                    var slot = line.Split('|');
                    slots.Add(new SlotInfo
                    {
                        Class = slot[0].ToString(),
                        Course = slot[1].ToString(),
                        Group = slot[2].ToString(),
                        Teacher = slot[3].ToString()
                    });
                    line = reader.ReadLine();
                }
                line = reader.ReadLine();
                var info = line.Split();
                var (begin, end) = GetSpan(info[0], info[1]);
                duration = int.Parse(info[2]);
                var rest = int.Parse(info[3]);
                var special_rest = new List<double>();
                for (var i = 4; i < info.Length; i += 2)
                {
                    var (start_rest, end_rest) = GetSpan(info[i], info[i + 1]);
                    special_rest.Add(start_rest);
                    special_rest.Add(end_rest);
                }
                while (begin < end)
                {
                    if (special_rest.Count == 0 || begin + duration < special_rest[0])
                    {
                        starts.Add(TimeSpan.FromMinutes(begin));
                        begin += duration + rest;
                    }
                    else
                    {
                        starts.Add(TimeSpan.FromMinutes(begin));
                        begin = special_rest[1];
                        special_rest.Remove(special_rest[0]);
                        special_rest.Remove(special_rest[0]);
                    }
                }
                reader.Close();
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException(".xlsx file was filled out wrongly");
            }
            return slots;
            //return (slots, starts, duration);
        }
    }
}