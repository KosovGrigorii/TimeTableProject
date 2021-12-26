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

        public byte[] GetPathToOutputFile(OutputExtension extension, string uid, IEnumerable<TimeSlot> timeslots)
        {
            var path = GetPath(extension, uid);
            var formatter = chooser.ChooseFormatter(extension);
            formatter.MakeOutputFile(path, converter.ConvertTimeslotsToDictionary(timeslots));
            return  File.ReadAllBytes(path);
        }

        private string GetPath(OutputExtension extension, string uid)
        {
            var fileName = $"{uid}.{extension.ToString().ToLower()}";
            return Path.Combine(Path.GetTempPath(), fileName);
        }
    }
}