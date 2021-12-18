using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimetableApplication;

namespace UserInterface
{
    public class InputProvider
    {
        private readonly Dictionary<Parsers, IInputParser> inputParsers;

        public InputProvider(Dictionary<Parsers, IInputParser> inputParsers)
        {
            this.inputParsers = inputParsers;
        }

        public IEnumerable<SlotInfo> ParseInput(Stream stream, Parsers extension)
        {
            var parser = inputParsers[extension];
            return parser.ParseFile(stream);
        }
    }
}