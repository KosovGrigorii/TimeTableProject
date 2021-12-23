using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using TimetableApplication;

namespace UserInterface
{
    public class XlsxInputParser : IInputParser
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

        public ParserExtension Extension => ParserExtension.xlsx;

        public IEnumerable<SlotInfo> ParseFile(Stream stream)
        {
            var slots = new List<SlotInfo>();
            var starts = new List<TimeSpan>();
            var duration = 90;
            stream.Position = 0;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            try
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == "-") break;
                    slots.Add(new SlotInfo
                    {
                        Course = reader.GetValue(0).ToString(),
                        Group = reader.GetValue(1).ToString(),
                        Teacher = reader.GetValue(2).ToString(), 
                        Room = reader.GetValue(3).ToString()
                        
                    });
                }
                reader.Read();
                var (begin, end) = GetSpan(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                duration = int.Parse(reader.GetValue(2).ToString());
                var rest = int.Parse(reader.GetValue(3).ToString());
                var special_rest = new List<double>();
                var temp = 4;
                while (reader.GetValue(temp) != null)
                {
                    var (start_rest, end_rest) = GetSpan(reader.GetValue(temp).ToString(), reader.GetValue(temp + 1).ToString());
                    special_rest.Add(start_rest);
                    special_rest.Add(end_rest);
                    temp += 2;
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