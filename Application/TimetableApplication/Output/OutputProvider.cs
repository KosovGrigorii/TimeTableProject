using System.Collections.Generic;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class OutputProvider
    {
        private readonly OutputConverter converter;
        private readonly ImplementationSelector<ParticularTimetable, byte[], IImplementation<ParticularTimetable, byte[]>> dictionary;

        public OutputProvider(
            OutputConverter converter,
            ImplementationSelector<ParticularTimetable, byte[], IImplementation<ParticularTimetable, byte[]>> dictionary)
        {
            this.converter = converter;
            this.dictionary = dictionary;
        }
        
        public IEnumerable<string> GetOutputExtensions()
            => dictionary.GetTypes();

        public byte[] GetOutputFileStream(string extension, IEnumerable<TimeSlot> timeslots)
            => dictionary.GetResult(extension, new (converter.ConvertTimeslotsToDictionary(timeslots)));
    }
}