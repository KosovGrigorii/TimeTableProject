using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class ParserChooser
    {
        private readonly IReadOnlyDictionary<ParserExtension, IInputParser> inputParsers;

        public ParserChooser(IEnumerable<IInputParser> inputParsers)
        {
            this.inputParsers = inputParsers.ToDictionary(x => x.Extension);
        }

        public IInputParser ChooseParser(ParserExtension parserExtensionExtension)
            => inputParsers[parserExtensionExtension];
    }
}