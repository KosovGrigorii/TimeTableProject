using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;

namespace TimetableApplication
{
    public class XlsxInputParser : IInputParser
    {
        public string Extension => ".xlsx";
        
        public IEnumerable<SlotInfo> ParseFile(Stream stream)
        {
            var slots = new List<SlotInfo>();
            stream.Position = 0;
            using var reader = ExcelReaderFactory.CreateReader(stream);
            try
            {
                while (reader.Read())
                {
                    slots.Add(new SlotInfo{
                        Course = reader.GetValue(0).ToString(), 
                        Group = reader.GetValue(1).ToString(), 
                        Teacher = reader.GetValue(2).ToString(),
                        Class = reader.GetValue(3).ToString()});
                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException(".xlsx file was filled out wrongly");
            }

            return slots;
        }
    }
}