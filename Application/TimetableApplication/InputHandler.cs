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
            parser.ParseFile(); //input передать в аргументы
            //? Либо база заполняется чразу IInputParser'ом, либо он возвращает
            //много сущностей, которыми заполняется база
        }
    }
}