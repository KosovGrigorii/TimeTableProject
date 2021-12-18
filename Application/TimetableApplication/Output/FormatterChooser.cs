using System.Collections.Generic;

namespace TimetableApplication
{
    public class FormatterChooser
    {
        private readonly IReadOnlyDictionary<OutputExtension, OutputFormatter> formatters;

        public FormatterChooser(IReadOnlyDictionary<OutputExtension, OutputFormatter> formatters)
        {
            this.formatters = formatters;
        }

        public OutputFormatter ChooseFormatter(OutputExtension extension) 
            => formatters[extension];
    }
}