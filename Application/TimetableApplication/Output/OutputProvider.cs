using System.Collections.Generic;
using System.IO;
using TimetableDomain;

namespace TimetableApplication
{
    public class OutputProvider
    {
        private readonly FormatterChooser chooser;
        private readonly OutputConverter converter;

        public OutputProvider(FormatterChooser chooser, OutputConverter converter)
        {
            this.chooser = chooser;
            this.converter = converter;
        }

        public byte[] GetOutputFileStream(string extension, IEnumerable<TimeSlot> timeslots)
        {
            var formatter = chooser.ChooseFormatter(extension);
            return formatter.MakeOutputFile(converter.ConvertTimeslotsToDictionary(timeslots));
        }
    }
}