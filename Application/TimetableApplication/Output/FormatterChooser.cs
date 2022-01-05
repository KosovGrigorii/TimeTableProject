using System.Collections.Generic;
using System.Linq;

namespace TimetableApplication
{
    public class FormatterChooser
    {
        private readonly IReadOnlyDictionary<string, IOutputFormatter> formatters;

        public FormatterChooser(IEnumerable<IOutputFormatter> outputFormatters)
        {
            formatters = outputFormatters.ToDictionary(x => x.Extension.Extension);
        }

        public IOutputFormatter ChooseFormatter(string extension) 
            => formatters[extension];
    }
}