using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TimetableApplication
{
    public class PdfOutputFormatter: IOutputFormatter
    {
        public OutputExtension Extension => OutputExtension.Pdf;

        public void MakeOutputFile(string filePath, Dictionary<string, string[,]> tables)
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
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
        }
    }
}