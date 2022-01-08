using System.Collections.Generic;
using Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace TimetableApplication
{
    public class XlsxOutputFormatter: IDictionaryType<ParticularTimetable, byte[]>
    {
        public string Name => "xlsx";

        public byte[] GetResult(ParticularTimetable parameters)
            => MakeOutputFile(parameters.Table);
        
        private byte[] MakeOutputFile(Dictionary<string, string[,]> tables)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            foreach (var (name, table) in tables)
            {
                var worksheet = package.Workbook.Worksheets.Add(name);
                
                for (var row = 1; row < 1 + table.GetLength(0); row++)
                for (var column = 1; column < 1 + table.GetLength(1); column++)
                    worksheet.Cells[row, column].Value = table[row - 1, column - 1];
                worksheet.Columns.AutoFit();
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            package.Save();
            return package.GetAsByteArray();
        }
    }
}
