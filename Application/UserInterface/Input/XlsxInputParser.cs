using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using TimetableApplication;
using UserInterface.Models;

namespace UserInterface
{
    public class XlsxInputParser : IInputParser
    {
        public ParserExtension Extension => ParserExtension.xlsx;
        
        public UserInput ParseFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var slots = new List<SlotInfo>();
            var times = new Times();
            stream.Position = 0;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            try
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == "-")
                    {
                        times = GetTimes(reader);
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
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException(".xlsx file was filled out wrongly");
            }

            return new UserInput() {CourseSlots = slots, TimeSchedule = times};
        }
        
        public Times GetTimes(IExcelDataReader reader)
        {
            reader.Read();
            var (begin, end) = GetSpan(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
            var (duration, rest) = (int.Parse(reader.GetValue(2).ToString()), int.Parse(reader.GetValue(3).ToString()));
            var special_rest = new List<double>();
            var times = new Times() { Duration = duration, LessonStarts = new List<TimeSpan>() };
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