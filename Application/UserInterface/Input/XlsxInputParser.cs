using System;
using System.Collections.Generic;
using TimetableApplication;
using System.IO;
using ExcelDataReader;

namespace UserInterface
{
    public class XlsxInputParser : IInputParser
    {
        public ParserExtension Extension => ParserExtension.Xlsx;
        
        public IEnumerable<SlotInfo> ParseFile(Stream stream)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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
                        Room = reader.GetValue(3).ToString()});
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