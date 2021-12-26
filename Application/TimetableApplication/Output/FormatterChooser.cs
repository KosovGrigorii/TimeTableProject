using System.Collections.Generic;
using System.Linq;

namespace TimetableApplication
{
    public class FormatterChooser
    {
        private readonly IReadOnlyDictionary<OutputExtension, IOutputFormatter> formatters;

        public FormatterChooser(IEnumerable<IOutputFormatter> outputFormatters)
        {
            formatters = outputFormatters.ToDictionary(x => x.Extension);
        }

        public IOutputFormatter ChooseFormatter(OutputExtension extension) 
            => formatters[extension];
    }
}