using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TimetableApplication
{
    public class InputHandler
    {
        public static void ParseInput(Stream inputStream, IInputParser parser)
        {
            var slots = parser.ParseFile(inputStream);
            DB.Slots = new HashSet<SlotInfo>();
            slots.Select(x => DB.Slots.Add(x)).ToList();
        }
    }
}