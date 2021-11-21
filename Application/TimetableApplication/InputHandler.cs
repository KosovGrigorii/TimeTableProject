using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class InputHandler
    {
        public static void ParseInput(MemoryStream inputStream, string inputFormat)
        {
            if (inputFormat != ".xlsx")
                throw new ArgumentException($"Cannot parse files with {inputFormat} extension.");
            var parser = new XlsxInputParser();
            var slots = parser.ParseFile(inputStream);
        }
    }
}