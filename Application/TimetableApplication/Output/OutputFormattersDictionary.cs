using System.Collections.Generic;
using System.Linq;

namespace TimetableApplication
{
    public class OutputFormattersDictionary
    {
        private readonly IEnumerable<IOutputFormatter> outputFormatters;
        
        public OutputFormattersDictionary(IEnumerable<IOutputFormatter> outputFormatters)
        {
            this.outputFormatters = outputFormatters;
        }

        public IEnumerable<string> GetExtensions()
            => outputFormatters.Select(f => f.Extension.Extension);

        public IReadOnlyDictionary<string, IOutputFormatter> GetDictionary()
            => outputFormatters.ToDictionary(f => f.Extension.Extension);
    }
}