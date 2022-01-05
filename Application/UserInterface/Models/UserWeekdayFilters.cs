using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserInterface.Models
{
    public class UserWeekdayFilters
    {
        public IEnumerable<string> Filters { get; init; }

        public IEnumerable<DayOfWeek> WeekDays { get; init; }
        public string Index { get; init; }
    }
}