using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace TimetableApplication
{
    public class OutputExecutor
    {
        private readonly OutputProvider outputProvider;
        private readonly TimeslotDbConverter converter;
        private readonly IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper;
        private readonly IEnumerable<string> extensions;

        public OutputExecutor(OutputProvider outputProvider,
            TimeslotDbConverter converter,
            IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper,
            IEnumerable<IOutputFormatter> formatters)
        {
            this.outputProvider = outputProvider;
            this.converter = converter;
            this.timeslotWrapper = timeslotWrapper;
            extensions = formatters.Select(f => f.Extension.Extension);
        }
        
        public IEnumerable<string> GetOutputExtensions()
            => extensions;

        public byte[] GetOutput(User user, string extension)
        {
            var timeslots = timeslotWrapper.ReadBy(user.Id).Select(x => converter.DbTimeslotToTimeslot(x));
            return outputProvider.GetOutputFileStream(extension, timeslots);
        }
    }
}