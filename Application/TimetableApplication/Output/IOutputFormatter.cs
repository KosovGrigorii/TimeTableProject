using System.Collections.Generic;

namespace TimetableApplication
{
    public interface IOutputFormatter
    {
        public OutputExtension Extension { get; }
        
        public void MakeOutputFile(string filePath, Dictionary<string, string[,]> tables);
    }
}