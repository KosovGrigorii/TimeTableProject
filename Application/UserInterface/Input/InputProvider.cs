using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TimetableApplication;

namespace UserInterface
{
    public class InputProvider
    {
        private ParserChooser chooser;

        public InputProvider(ParserChooser chooser)
        {
            this.chooser = chooser;
        }

        public IEnumerable<SlotInfo> ParseInput(IFormFile file, ParserExtension extension)
        {
            var parser = chooser.ChooseParser(extension);
            return parser.ParseFile(file);
        }
    }
}