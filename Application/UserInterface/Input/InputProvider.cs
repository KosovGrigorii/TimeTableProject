using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public UserInput ParseInput(Stream stream, ParserExtension extension)
        {
            var parser = chooser.ChooseParser(extension);
            return parser.ParseFile(stream);
        }
    }
}