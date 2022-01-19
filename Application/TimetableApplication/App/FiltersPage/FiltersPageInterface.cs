using System.Collections.Generic;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class FiltersPageInterface
    {
        private FilterNamesGetter filtersGetter;
        private readonly TimetableTaskLauncher taskLauncher;

        private readonly
            ImplementationSelector<AlgoritmInput, IEnumerable<TimeSlot>,
                IImplementation<AlgoritmInput, IEnumerable<TimeSlot>>> algorithmsDictionary;
        
        public FiltersPageInterface(FilterNamesGetter filtersGetter, ImplementationSelector<AlgoritmInput, IEnumerable<TimeSlot>, IImplementation<AlgoritmInput, IEnumerable<TimeSlot>>> algorithmsDictionary, TimetableTaskLauncher taskLauncher)
        {
            this.filtersGetter = filtersGetter;
            this.algorithmsDictionary = algorithmsDictionary;
            this.taskLauncher = taskLauncher;
        }

        public IEnumerable<string> GetAlgorithmNames()
            => algorithmsDictionary.GetTypes();

        public IEnumerable<string> GetTeachersNameForFilters(User user)
            => filtersGetter.GetTeachers(user);

        public async void AddTimetableMakingTask(User user, string algorithm, IEnumerable<Filter> filters)
            => await taskLauncher.MakeTimetable(user, algorithm, filters);
    }
}