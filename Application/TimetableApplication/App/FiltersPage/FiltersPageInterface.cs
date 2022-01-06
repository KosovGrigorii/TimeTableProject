using System.Collections.Generic;

namespace TimetableApplication
{
    public class FiltersPageInterface
    {
        private Algorithms algorithms;
        private FilterNamesGetter filtersGetter;
        private readonly TimetableTaskAdder timetableTaskAdder;
        
        public FiltersPageInterface(Algorithms algorithms, FilterNamesGetter filtersGetter, TimetableTaskAdder timetableTaskAdder)
        {
            this.algorithms = algorithms;
            this.filtersGetter = filtersGetter;
            this.timetableTaskAdder = timetableTaskAdder;
        }
        
        public IEnumerable<string> GetAlgorithmNames()
        => algorithms.NamesList;

        public IEnumerable<string> GetTeachersNameForFilters(User user)
            => filtersGetter.GetTeachers(user);

        public void AddTimetableMakingTask(User user, string algorithm, IEnumerable<Filter> filters)
            => timetableTaskAdder.AddTaskFor(user, algorithm, filters);
    }
}