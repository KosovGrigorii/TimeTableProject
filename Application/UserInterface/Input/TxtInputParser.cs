using System;
using System.Collections.Generic;
using System.IO;
using TimetableApplication;



namespace UserInterface
{
    class TxtInputParser : IInputParser
    {
        public ParserExtension Extension => ParserExtension.txt;

        public IEnumerable<SlotInfo> ParseFile(Stream stream)
        {
            var slots = new List<SlotInfo>();
            var times = new Times();
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            try
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    if (line == "-")
                    {
                        times = GetTimes(reader);
                        break;
                    }
                    var slot = line.Split('|');
                    slots.Add(new SlotInfo
                    {
                        Course = slot[0].ToString(),
                        Group = slot[1].ToString(),
                        Teacher = slot[2].ToString(),
                        Room = slot[3].ToString()
                    });
                    line = reader.ReadLine();
                }
                reader.Close();
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException(".xlsx file was filled out wrongly");
            }
            return slots;
            //return (slots, times);
        }

        public Times GetTimes(StreamReader reader)
        {
            var times = new Times() { times = new List<Tuple<TimeSpan, int>>() };
            var info = reader.ReadLine().Split();
            var (begin, end) = GetSpan(info[0], info[1]);
            var (duration, rest) = (int.Parse(info[2]), int.Parse(info[3]));
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
                    times.times.Add(new Tuple<TimeSpan, int>(TimeSpan.FromMinutes(begin), duration));
                    begin += duration + rest;
                    continue;
                }
                times.times.Add(new Tuple<TimeSpan, int>(TimeSpan.FromMinutes(begin), (int)(special_rest[0] - begin)));
                begin = special_rest[1];
                special_rest.RemoveRange(0, 2);
            }
            return times;
        }

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
    }
}