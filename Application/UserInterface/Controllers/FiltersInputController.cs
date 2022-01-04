using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimetableApplication;
using TimetableDomain;
using UserInterface.Models;

namespace UserInterface
{
    public class FiltersInputController : Controller
    {
        private readonly FilterInterface filterInterface;
        private readonly TimetableMaker timetableMaker;
        private readonly IReadOnlyDictionary<string, Func<IEnumerable<string>, string, PartialViewResult>> filterToInputForm;

        public FiltersInputController(FilterInterface filterInterface, TimetableMaker timetableMaker)
        {
            this.filterInterface = filterInterface;
            this.timetableMaker = timetableMaker;
            filterToInputForm = new Dictionary<string, Func<IEnumerable<string>, string, PartialViewResult>>()
            {
                { "Working days amount", GetWorkingDaysCountFilter },
                { "Choose working days in week", GetSpecifiedWorkingDaysFilter }
            };
        }
        
        public IActionResult FiltersInput(string uid)
        {
            var model = new FiltersPageData() { Algorithms = new SelectList(filterInterface.GetAlgorithmNames()), UserID = uid };
            return View(model);
        }
        
        public PartialViewResult _FilterChoosingForm(string userId, string elementId)
        {
            var model = new FilterChoose 
            { 
                Categories = new SelectList(filterToInputForm.Keys), 
                UserId = userId, 
                FilterId = elementId
            };
            return PartialView(model);
        }

        public PartialViewResult ChooseSingleFilter(string filterName, string userId, string elementId)
        {
            var user = new User() {Id = userId};
            var specifiedFilters = filterInterface.GetTeachers(user);
            return filterToInputForm[filterName](specifiedFilters, elementId);
        }
        
        public PartialViewResult GetWorkingDaysCountFilter(IEnumerable<string> specifiedFilters, string elementId)
        {
            var userFilters = new UserFilters() {Filters = specifiedFilters, Index = elementId};
            return PartialView("_DaysCountFilter", userFilters);
        }

        public PartialViewResult GetSpecifiedWorkingDaysFilter(IEnumerable<string> specifiedFilters, string elementId)
        {
            var weekDayFilters = new UserWeekdayFilters() 
            { 
                WeekDays = Enum.GetValues(typeof(DayOfWeek))
                    .OfType<DayOfWeek>()
                    .Skip(1),
                Filters = specifiedFilters, 
                Index = elementId
            };
            return PartialView("_DaysFilter", weekDayFilters);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string algorithm, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.DaysCount, x.Days));
            var algoAvailable = timetableMaker.Algoorithms.Contains(algorithm);
            if (!algoAvailable)
                return View("ErrorPage");
            
            var user = new User() {Id = uid};
            timetableMaker.MakeTimetable(user, algorithm, applicationFilters);
            return RedirectToAction("ToLoadingPage", new { uid = uid });
        }
        
        public IActionResult ToLoadingPage(string uid)
        {
            return RedirectToRoutePermanent("default", new
            {
                controller = "LoadingPage", action = "Index", uid = uid
            });
        }
    }
}