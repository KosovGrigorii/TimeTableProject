using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace UserInterface
{
    public class InputProvider
    {
        private ParserChooser chooser;
        private ParserExtensions extensions;

        public InputProvider(ParserChooser chooser, ParserExtensions extensions)
        {
            this.chooser = chooser;
            this.extensions = extensions;
        }

        public IEnumerable<string> GetExtensions()
            => extensions.Extensions;

        public bool IsExtensionAvailable(string extension)
            => extensions.Extensions.Contains(extension);

        public UserInput ParseInput(IFormFile file, string extension)
        {
            var parser = chooser.ChooseParser(extension);
            return parser.ParseFile(file);
        }
    }
}