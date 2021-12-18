using System.Collections.Generic;

namespace UserInterface
{
    public class ParserChooser
    {
        private readonly Dictionary<ParserExtension, IInputParser> inputParsers;

        public ParserChooser(Dictionary<ParserExtension, IInputParser> inputParsers)
        {
            this.inputParsers = inputParsers;
        }

        public IInputParser ChooseParser(ParserExtension parserExtensionExtension)
            => inputParsers[parserExtensionExtension];
    }
}