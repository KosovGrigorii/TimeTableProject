using System.Collections.Generic;
using System.Linq;

namespace TimetableApplication
{
    public class FormatterChooser
    {
        private readonly IReadOnlyDictionary<OutputExtension, OutputFormatter> formatters;

        public FormatterChooser(IEnumerable<OutputFormatter> outputFormatters)
        {
            formatters = outputFormatters.ToDictionary(x => x.Extension);
        }

        public OutputFormatter ChooseFormatter(OutputExtension extension) 
            => formatters[extension];
    }
}