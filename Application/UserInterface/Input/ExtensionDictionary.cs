using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class ExtensionDictionary
    {
        private readonly IReadOnlyDictionary<string, IInputParser> inputParsers;

        public ExtensionDictionary(IEnumerable<IInputParser> inputParsers)
        {
            this.inputParsers = inputParsers.ToDictionary(x => x.Extension.Extension);
        }

        public IEnumerable<string> GetExtensions()
        => inputParsers.Keys;

        public IInputParser GetParser(string parserExtension)
        => inputParsers[parserExtension];
    }
}
