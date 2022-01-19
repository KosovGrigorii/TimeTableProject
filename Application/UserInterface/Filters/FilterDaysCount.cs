using Infrastructure;
using UserInterface.Models;

namespace UserInterface
{
    class FilterDaysCount : IImplementation<FilterGetterParameters, FilterPartialViewData>
    {
        public string Name => "Working days amount";
        public FilterPartialViewData GetResult(FilterGetterParameters parameters)
        {
            var (teachers, elementId) = parameters;
            var userFilters = new UserFilters() {Filters = teachers, Index = elementId};
            return new FilterPartialViewData("_DaysCountFilter", userFilters);
        }
    }
}