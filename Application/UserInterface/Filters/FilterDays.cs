using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;

namespace UserInterface
{
    class FilterDays: IDictionaryType<FilterGetterParameters, FilterPartialViewData>
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