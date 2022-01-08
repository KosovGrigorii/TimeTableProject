using System.Collections.Generic;

namespace TimetableApplication
{
    public class OutputExecutor
    {
        private readonly OutputProvider outputProvider;
        private readonly TimetableResultsInterface timetableResults;

        public OutputExecutor(
            OutputProvider outputProvider,
            TimetableResultsInterface timetableResults)
        {
            this.outputProvider = outputProvider;
            this.timetableResults = timetableResults;
        }

        public IEnumerable<string> GetOutputExtensions()
            => outputProvider.GetOutputExtensions();

        public bool IsTimetableReadyFor(User user)
            => timetableResults.IsTimetableReadyForUser(user);

        public byte[] GetOutput(User user, string extension)
        {
            var timeslots = timetableResults.GetResultForUser(user);
            return outputProvider.GetOutputFileStream(extension, timeslots);
        }
    }
}