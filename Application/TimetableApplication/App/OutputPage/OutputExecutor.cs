using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace TimetableApplication
{
    public class OutputExecutor
    {
        private readonly OutputExtentions extentions;
        private readonly OutputProvider outputProvider;
        private readonly TimetableResultsInterface timetableResults;

        public OutputExecutor(
            OutputExtentions extentions, 
            OutputProvider outputProvider,
            TimetableResultsInterface timetableResults)
        {
            this.extentions = extentions;
            this.outputProvider = outputProvider;
            this.timetableResults = timetableResults;
        }
        
        public IEnumerable<string> GetOutputExtensions()
            => extentions.Extensions;

        public bool IsTimetableReadyFor(User user)
            => timetableResults.IsTimetableReadyForUser(user);

        public byte[] GetOutput(User user, string extension)
        {
            var timeslots = timetableResults.GetResultForUser(user);
            return outputProvider.GetOutputFileStream(extension, timeslots);
        }
    }
}