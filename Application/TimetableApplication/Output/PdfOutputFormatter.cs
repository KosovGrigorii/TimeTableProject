using System.Collections.Generic;
using System.IO;
using Infrastructure;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TimetableApplication
{
    public class PdfOutputFormatter: IImplementation<ParticularTimetable, byte[]>
    {
        public string Name => "pdf";

        public byte[] GetResult(ParticularTimetable parameters)
            => MakeOutputFile(parameters.Table);

        internal byte[] MakeOutputFile(Dictionary<string, string[,]> tables)
        {
            var doc = new Document();
            using var resultStream = new MemoryStream();
            PdfWriter.GetInstance(doc, resultStream);
            doc.Open();
            var count = 0;
            foreach (var (name, table) in tables)
            {
                var sheet = new PdfPTable(8);
                var title = new PdfPCell(new Phrase(name))
                {
                    Colspan = 8
                };
                sheet.AddCell(title);
                foreach (var cell in table)
                    sheet.AddCell(cell);
                doc.Add(sheet);
                if (++count == tables.Count) 
                    continue;
                doc.Add(new Phrase("\n"));
            }
            doc.Close();
            return resultStream.ToArray();
        }
    }
}