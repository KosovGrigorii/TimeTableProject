using System.Collections.Generic;
using System.Linq;

namespace TimetableApplication
{
    public class FormatterChooser
    {
        private readonly IReadOnlyDictionary<string, IOutputFormatter> formatters;

        public FormatterChooser(OutputFormattersDictionary dictionary)
        {
            formatters = dictionary.GetDictionary();
        }

        public IOutputFormatter ChooseFormatter(string extension) 
            => formatters[extension];
    }
}