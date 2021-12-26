using System.Collections.Generic;
using TimetableApplication;
using System.IO;

namespace UserInterface
{
    public interface IInputParser
    {
        ParserExtension Extension {get;}
        UserInput ParseFile(Stream stream);
    }
}