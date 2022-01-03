using System;
using System.Linq;
using Infrastructure;

namespace TimetableApplication
{
    public class OutputExecutor
    {
        private readonly OutputProvider outputProvider;
        private readonly TimeslotDbConverter converter;
        private readonly IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper;
        
        public OutputExecutor(OutputProvider outputProvider,
            TimeslotDbConverter converter,
            IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper)
        {
            this.outputProvider = outputProvider;
            this.converter = converter;
            this.timeslotWrapper = timeslotWrapper;
        }
        
        public Array GetOutputExtensions()
            => Enum.GetValues(typeof(OutputExtension));

        public byte[] GetOutput(User user, OutputExtension extension)
        {
            var timeslots = timeslotWrapper.ReadBy(user.Id).Select(x => converter.DbTimeslotToTimeslot(x));
            return outputProvider.GetOutputFileStream(extension, timeslots);
        }
    }
}