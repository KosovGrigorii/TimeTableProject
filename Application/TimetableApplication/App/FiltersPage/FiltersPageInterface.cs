using System;
using System.Collections.Generic;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class FiltersPageInterface
    {
        private FilterNamesGetter filtersGetter;
        private readonly TimetableTaskAdder timetableTaskAdder;

        private readonly
            DependenciesDictionary<AlgoritmInput, IEnumerable<TimeSlot>,
                IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>> algorithmsDictionary;
        
        public FiltersPageInterface(FilterNamesGetter filtersGetter, TimetableTaskAdder timetableTaskAdder, DependenciesDictionary<AlgoritmInput, IEnumerable<TimeSlot>, IDictionaryType<AlgoritmInput, IEnumerable<TimeSlot>>> algorithmsDictionary)
        {
            this.filtersGetter = filtersGetter;
            this.timetableTaskAdder = timetableTaskAdder;
            this.algorithmsDictionary = algorithmsDictionary;
        }

        public IEnumerable<string> GetAlgorithmNames()
            => algorithmsDictionary.GetTypes();

        public IEnumerable<string> GetTeachersNameForFilters(User user)
            => filtersGetter.GetTeachers(user);

        public void AddTimetableMakingTask(User user, string algorithm, IEnumerable<Filter> filters)
            => timetableTaskAdder.AddTaskFor(user, algorithm, filters);
    }
}