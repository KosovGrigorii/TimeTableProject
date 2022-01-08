using System.Collections.Generic;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;

namespace UserInterface
{
    class FilterDaysCount : IDictionaryType<FilterGetterParameters, FilterPartialViewData>
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