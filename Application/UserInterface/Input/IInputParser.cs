using System.Collections.Generic;
using TimetableApplication;
using System.IO;

namespace UserInterface
{
    public interface IInputParser
    {
        Parsers Extension {get;}
        IEnumerable<SlotInfo> ParseFile(Stream stream);
    }
}