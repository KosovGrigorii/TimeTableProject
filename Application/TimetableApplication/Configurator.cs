using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimetableDomain;

namespace TimetableApplication
{
    public class Configurator
    {
        private readonly Dictionary<string, IInputParser> parsers;
        private readonly Dictionary<string, ITimetableMaker> algorithms;
        private readonly Dictionary<string, OutputFormatter> formatters;
        
        public Configurator(IEnumerable<IInputParser> parsers,
            IEnumerable<ITimetableMaker> algorithms,
            IEnumerable<OutputFormatter> formatters)
        {
            this.parsers = parsers.ToDictionary(x => x.Extension, x => x);
            this.algorithms = algorithms.ToDictionary(x => x.Name, x => x);
            this.formatters = formatters.ToDictionary(x => x.Extension, x => x);
        }

        public void Input(Stream stream, string extention)
        {
            var parser = parsers[extention];
            InputHandler.ParseInput(stream, parser);
        }

        public void MakeTimetable(IEnumerable<Filter> filters)
        {
            var algo = algorithms["Genetic"];
            TimetableMakingController.StartMakingTimeTable(algo);
        }

        public void GetOutput()
        {
            var formatter = formatters[".xlsx"];
            throw new NotImplementedException();
        }
    }
}