using System;
using System.Collections.Generic;
using System.IO;
using TimetableApplication;

namespace UserInterface
{
    class TxtInputParser : IInputParser
    {
        public ParserExtension Extension => ParserExtension.Txt;

        public IEnumerable<SlotInfo> ParseFile(Stream stream)
        {
            var slots = new List<SlotInfo>();
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            try
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    var slot = line.Split('|');
                    slots.Add(new SlotInfo
                    {
                        Room = slot[0].ToString(),
                        Course = slot[1].ToString(),
                        Group = slot[2].ToString(),
                        Teacher = slot[3].ToString()
                    });
                    line = reader.ReadLine();
                }
                // reader.Close(); Нужно ли закрывать?
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException(".xlsx file was filled out wrongly");
            }
            return slots;
        }
    }
}
