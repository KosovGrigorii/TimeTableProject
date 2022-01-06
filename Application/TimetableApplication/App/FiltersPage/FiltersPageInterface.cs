using System;
using System.Collections.Generic;

namespace TimetableApplication
{
    public class FiltersPageInterface
    {
        private Lazy<IEnumerable<string>> algorithmNames;
        private FilterNamesGetter filtersGetter;
        private readonly TimetableTaskAdder timetableTaskAdder;
        
        public FiltersPageInterface(FilterNamesGetter filtersGetter, TimetableTaskAdder timetableTaskAdder, AlgorithmsDictionary algorithmsDictionary)
        {
            this.filtersGetter = filtersGetter;
            this.timetableTaskAdder = timetableTaskAdder;
            algorithmNames = new Lazy<IEnumerable<string>>(algorithmsDictionary.GetAlgorithms());
        }

        public IEnumerable<string> GetAlgorithmNames()
            => algorithmNames.Value;

        public IEnumerable<string> GetTeachersNameForFilters(User user)
            => filtersGetter.GetTeachers(user);

        public void AddTimetableMakingTask(User user, string algorithm, IEnumerable<Filter> filters)
            => timetableTaskAdder.AddTaskFor(user, algorithm, filters);
    }
}