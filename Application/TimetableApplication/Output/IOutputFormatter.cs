using System.Collections.Generic;
using System.IO;

namespace TimetableApplication
{
    public interface IOutputFormatter
    {
        public OutputExtension Extension { get; }
        
        public byte[] MakeOutputFile(Dictionary<string, string[,]> tables);
    }
}