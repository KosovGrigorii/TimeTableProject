using System.Collections.Generic;
using System.IO;
using TimetableDomain;

namespace TimetableApplication
{
    public class OutputProvider
    {
        private FormatterChooser chooser;
        private OutputConverter converter;

        public OutputProvider(FormatterChooser chooser, OutputConverter converter)
        {
            this.chooser = chooser;
            this.converter = converter;
        }

        public byte[] GetOutputFileStream(OutputExtension extension, string uid, IEnumerable<TimeSlot> timeslots)
        {
            var formatter = chooser.ChooseFormatter(extension);
            return formatter.MakeOutputFile(converter.ConvertTimeslotsToDictionary(timeslots));
        }

        private string GetPath(OutputExtension extension, string uid)
        {
            var fileName = $"{uid}.{extension.ToString().ToLower()}";
            return Path.Combine(Path.GetTempPath(), fileName);
        }
    }
}