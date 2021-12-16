using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TimetableApplication
{
    public class InputHandler
    {
        public static void ParseInput(string uid, Stream inputStream, IInputParser parser)
        {
            var slots = parser.ParseFile(inputStream);
            UserToData.SetInputInfo(uid, slots);
        }
    }
}