using System.Collections.Generic;
using System.IO;

namespace TimetableApplication
{
    public interface IInputParser
    {
        string Extension {get;}
        IEnumerable<SlotInfo> ParseFile(Stream stream);
    }
}