using System;
using System.Linq;
using Infrastructure;
using UserInterface.Models;

namespace UserInterface
{
    class FilterDays: IImplementation<FilterGetterParameters, FilterPartialViewData>
    {
        public string Name => "Choose working days in week";
        public FilterPartialViewData GetResult(FilterGetterParameters parameters)
        {
            var (teachers, elementId) = parameters;
            var weekDayFilters = new UserWeekdayFilters() 
            { 
                WeekDays = Enum.GetValues(typeof(DayOfWeek))
                    .OfType<DayOfWeek>()
                    .Skip(1),
                Filters = teachers, 
                Index = elementId
            };
            return new FilterPartialViewData("_DaysFilter", weekDayFilters);
        }
    }
}