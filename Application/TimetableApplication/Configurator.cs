using TimetableCommonClasses;
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
        private readonly InstanceCreator commonInstanceCreator = new InstanceCreator();
        
        public Configurator(IEnumerable<IInputParser> parsers,
            IEnumerable<ITimetableMaker> algorithms,
            IEnumerable<OutputFormatter> formatters)
        {
            this.parsers = parsers.ToDictionary(x => x.Extension, x => x);
            this.algorithms = algorithms.ToDictionary(x => x.Name, x => x);
            this.formatters = formatters.ToDictionary(x => x.Extension, x => x);
        }

        public void Input(string uid, Stream stream, string extension)
        {
            var parser = parsers[extension];
            UserToData.AddUser(uid);
            InputHandler.ParseInput(uid, stream, parser);
        }
        public void MakeTimetable(string uid, IEnumerable<Filter> filters)
        {
            var algo = algorithms["Genetic"];
            TimetableMakingController.StartMakingTimeTable(uid, algo, filters);
        }
        
        public string GetOutputFile(string uid, string extension)
        {
            var formatter = formatters[extension];
            var fileName = uid + extension;
            var path =  Path.Combine(Path.GetTempPath(), fileName);
            var timeslots = UserToData.GetTimeslots(uid);
            formatter.GetOutputFile(path, timeslots);
            return path;
        }
    }
}