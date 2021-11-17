using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TimetableDomain;

namespace TimetableApplication
{
    public class XlsxOutputFormatter: OutputFormatter
    {
        public override FileInfo GetOutputFile(IEnumerable<TimeSlot> timeSlots)
        {
            var (schedule, bells) = GetTables(timeSlots.ToList());
            var path = "";
            var file = new FileInfo(path + "my_xlsx_file.xlsx");
            file.Create();
            using var package = new ExcelPackage(file);
            foreach (var (name, table) in schedule)
            {
                var worksheet = package.Workbook.Worksheets.Add(name);
                for (var day = 1; day < 8; day++)
                    worksheet.Cells[1, day + 1].Value = Enum.GetName(typeof(DayOfWeek), day % 7);
                for (var bell = 2; bell < 2 + bells.Count; bell++)
                    worksheet.Cells[bell, 1].Value = $"{bells[bell - 2].Item1} — {bells[bell - 2].Item2}";
                for (var row = 2; row < 2 + table.GetLength(0); row++)
                for (var column = 2; column < 2 + table.GetLength(1); column++)
                    worksheet.Cells[row, column].Value = table[row - 2, column - 2];
                worksheet.Columns.AutoFit();
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            package.Save();
            return file;
        }
    }
}
