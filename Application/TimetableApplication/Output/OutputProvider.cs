using System.Collections.Generic;
using System.IO;
using System.Linq;
using Accord;
using TimetableDomain;

namespace TimetableApplication
{
    public class OutputProvider
    {
        private readonly IReadOnlyDictionary<Formatters, OutputFormatter> formatters;
        
        public OutputProvider(IReadOnlyDictionary<Formatters, OutputFormatter> formatters)
        {
            this.formatters = formatters;
        }

        public string GetOutputPath(Formatters extension, string uid, IEnumerable<TimeSlot> timeslots)
        {
            var fileName = $"{uid}.{extension.ToString().ToLower()}";
            var path =  Path.Combine(Path.GetTempPath(), fileName);
            
            var formatter = formatters[extension];
            formatter.GetOutputFile(path, timeslots);
            return path;
        }
    }
}