using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace TimetableApplication
{
    public class InputHandler
    {
        public static void ParseInput(byte[] input, string inputFormat)
        {
            var parser = new XlsxInputParser(); //inputFormat
            //var entities = 
            parser.ParseFile(input);
            var entities = new List<DbEntity>();
            DBShell.Input(entities);
        }
    }
}