using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimetableDomain;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TimetableApplication
{
    public class PdfOutputFormatter: OutputFormatter
    {
        public override OutputExtension Extension => OutputExtension.Pdf;

        public override void MakeOutputFile(string filePath, IEnumerable<TimeSlot> timeSlots)
        {
            var (schedule, bells) = ConvertTimeSlotsToDictionaries(timeSlots.ToList());
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();
            var count = 0;
            foreach (var (name, table) in schedule)
            {
                var sheet = new PdfPTable(8);
                var title = new PdfPCell(new Phrase(name))
                {
                    Colspan = 8
                };
                sheet.AddCell(title);
                sheet.AddCell("");
                for (var day = 1; day < 8; day++)
                    sheet.AddCell(Enum.GetName(typeof(DayOfWeek), day % 7));
                for (var i = 0; i < table.GetLength(0); i++)
                {
                    sheet.AddCell($"{bells[i].Item1} — {bells[i].Item2}");
                    for (var j = 0; j < 7; j++)
                        sheet.AddCell(table[i, j]);
                }
                doc.Add(sheet);
                if (++count == schedule.Count) 
                    continue;
                doc.Add(new Phrase("\n"));
            }
            doc.Close();
        }
    }
}