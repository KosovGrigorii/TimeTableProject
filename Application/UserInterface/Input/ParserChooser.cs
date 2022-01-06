using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class ParserChooser
    {
        private readonly ExtensionDictionary extensionDictionary;

        public ParserChooser(IEnumerable<IInputParser> inputParsers)
        {
            extensionDictionary = new ExtensionDictionary(inputParsers);
        }

        public IInputParser ChooseParser(string parserExtension)
            => extensionDictionary.GetParser(parserExtension);
    }
}