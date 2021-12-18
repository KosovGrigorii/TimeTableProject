using System.Collections.Generic;
using System.IO;
using System.Linq;
using Accord;
using TimetableDomain;

namespace TimetableApplication
{
    public class OutputProvider
    {
        private FormatterChooser chooser;
        
        public OutputProvider(IReadOnlyDictionary<OutputExtension, OutputFormatter> formatters)
        {
            chooser = new FormatterChooser(formatters);
        }

        public string GetPathToOutputFile(OutputExtension extension, string uid, IEnumerable<TimeSlot> timeslots)
        {
            var path = GetPath(extension, uid);
            var formatter = chooser.ChooseFormatter(extension);
            formatter.MakeOutputFile(path, timeslots);
            return path;
        }

        private string GetPath(OutputExtension extension, string uid)
        {
            var fileName = $"{uid}.{extension.ToString().ToLower()}";
            return Path.Combine(Path.GetTempPath(), fileName);
        }
    }
}