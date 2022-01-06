using System.Collections.Generic;
using System.Linq;

namespace TimetableApplication
{
    public class OutputExtentions
    {
        public IEnumerable<string> Extensions { get; }

        public OutputExtentions(IEnumerable<IOutputFormatter> formatters)
        {
            Extensions = formatters.Select(f => f.Extension.Extension);
        }
    }
}