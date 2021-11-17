using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class InputHandler
    {
        public static void ParseInput(byte[] input, string inputFormat)
        {
            var parser = new XlsxInputParser(); //inputFormat
            //var entities = 
            parser.ParseFile(input);
            var entities = new List<Entity>();
            //DBShell.Input(entities); добавить в базу сущностей 
        }
    }
}