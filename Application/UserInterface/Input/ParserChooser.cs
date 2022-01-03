using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class ParserChooser
    {
        private readonly IReadOnlyDictionary<string, IInputParser> inputParsers;

        public ParserChooser(IEnumerable<IInputParser> inputParsers)
        {
            this.inputParsers = inputParsers.ToDictionary(x => x.Extension);
        }

        public IInputParser ChooseParser(string parserExtension)
            => inputParsers[parserExtension];
    }
}