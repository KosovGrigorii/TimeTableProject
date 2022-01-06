using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace UserInterface
{
    public class InputProvider
    {
        private readonly ParserChooser chooser;

        private readonly ExtensionDictionary extensionDictionary;

        public InputProvider(ParserChooser chooser, ExtensionDictionary extensionDictionary)
        {
            this.chooser = chooser;
            this.extensionDictionary = extensionDictionary;
        }

        public IEnumerable<string> GetExtensions()
            => extensionDictionary.GetExtensions();

        public bool IsExtensionAvailable(string extension)
            => extensionDictionary.GetExtensions().Contains(extension);

        public UserInput ParseInput(IFormFile file, string extension)
        {
            var parser = chooser.ChooseParser(extension);
            return parser.ParseFile(file);
        }
    }
}