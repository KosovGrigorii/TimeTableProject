using System.Collections.Generic;
using System.IO;

namespace TimetableApplication
{
    public interface IInputParser
    {
        IEnumerable<SlotInfo> ParseFile(MemoryStream stream);
    }
}