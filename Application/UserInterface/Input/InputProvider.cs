using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace UserInterface
{
    public class InputProvider
    {
        private readonly ParserChooser chooser;
        //private readonly IEnumerable<string> extensions;

        public InputProvider(ParserChooser chooser) //, IEnumerable<IInputParser> inputParsers)
        {
            this.chooser = chooser;
            //extensions = chooser.GetExtension();  //inputParsers.Select(x => x.Extension.Extension);
        }

        public IEnumerable<string> GetExtensions()
            => chooser.GetExtension();

        public bool IsExtensionAvailable(string extension)
            => chooser.GetExtension().Contains(extension);

        public UserInput ParseInput(IFormFile file, string extension)
        {
            var parser = chooser.ChooseParser(extension);
            return parser.ParseFile(file);
        }
    }
}