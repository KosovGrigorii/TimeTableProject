using System;
using System.Collections.Generic;
using System.IO;
using TimetableDomain;

namespace TimetableApplication
{
    public class OutputhHandler
    {
        public static FileInfo GetOutputFilePath(string format)
        {
            var formatter = new XlsxOutputFormatter();
            //Запрос всех таймслотов из базы
            var timeslots = new List<TimeSlot>();
            if (format != "xlsx")
                throw new Exception("Format is not supported");
            return formatter.GetOutputFile(timeslots);
        }
    }
}