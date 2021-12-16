using System.Collections.Generic;
using System.IO;

namespace TimetableApplication
{
    public class SomeParser : IInputParser
    {
        public string Extension => "parser";
        
        public IEnumerable<SlotInfo> ParseFile(Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}