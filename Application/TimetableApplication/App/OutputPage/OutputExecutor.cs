using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace TimetableApplication
{
    public class OutputExecutor
    {
        private readonly OutputProvider outputProvider;
        private readonly TimetableResultsInterface timetableResults;
        private readonly Lazy<IEnumerable<string>> extensions;

        public OutputExecutor(
            OutputProvider outputProvider,
            TimetableResultsInterface timetableResults, OutputFormattersDictionary dictionary)
        {
            this.outputProvider = outputProvider;
            this.timetableResults = timetableResults;
            extensions = new Lazy<IEnumerable<string>>(dictionary.GetExtensions());
        }
        
        public IEnumerable<string> GetOutputExtensions()
            => extensions.Value;

        public bool IsTimetableReadyFor(User user)
            => timetableResults.IsTimetableReadyForUser(user);

        public byte[] GetOutput(User user, string extension)
        {
            var timeslots = timetableResults.GetResultForUser(user);
            return outputProvider.GetOutputFileStream(extension, timeslots);
        }
    }
}