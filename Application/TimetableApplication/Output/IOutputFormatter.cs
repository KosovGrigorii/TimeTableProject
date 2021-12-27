using System.Collections.Generic;
using System.IO;

namespace TimetableApplication
{
    public interface IOutputFormatter
    {
        public OutputExtension Extension { get; }
        
        public Stream MakeOutputFile(Dictionary<string, string[,]> tables);
    }
}