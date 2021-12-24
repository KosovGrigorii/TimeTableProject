using System.Collections.Generic;
using TimetableApplication;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace UserInterface
{
    public interface IInputParser
    {
        ParserExtension Extension {get;}
        IEnumerable<SlotInfo> ParseFile(IFormFile file);
    }
}