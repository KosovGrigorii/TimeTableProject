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

        public FiltersInputController(FilterInterface filterInterface, TimetableMaker timetableMaker)
        {
            this.filterInterface = filterInterface;
            this.timetableMaker = timetableMaker;
        }
        
        public IActionResult FiltersInput(string uid)
        {
            var model = new FiltersPageData() { Algorithms = new SelectList(filterInterface.GetAlgorithmNames()), UserID = uid };
            return View(model);
        }
        
        public PartialViewResult _FilterChoosingForm(string userId, string elementId)
        {
            var model = new FilterChoose {UserId = userId, FilterId = elementId};
            return PartialView(model);
        }

        public PartialViewResult ChooseSingleFilter(string filterName, string userId, string elementId)
        {
            var user = new User() {Id = userId};
            var specifiedFilters = filterInterface.GetTeachers(user);
            if (filterName == "Working days amount")
                return GetWorkingDaysCountFilter(specifiedFilters, elementId);
            return GetSpecifiedWorkingDaysFilter(specifiedFilters, elementId);
        }
        
        public PartialViewResult GetWorkingDaysCountFilter(IEnumerable<string> specifiedFilters, string elementId)
        {
            var userFilters = new UserFilters() {Filters = specifiedFilters, Index = elementId};
            return PartialView("_DaysCountFilter", userFilters);
        }

        public PartialViewResult GetSpecifiedWorkingDaysFilter(IEnumerable<string> specifiedFilters, string elementId)
        {
            var weekDayFilters = new UserWeekdayFilters() {Filters = specifiedFilters, Index = elementId};
            return PartialView("_DaysFilter", weekDayFilters);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string algorithm, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.DaysCount, x.Days));
            var algoConverted = Enum.TryParse<Algorithm>(algorithm, out var algo);

            if (!algoConverted)
                return View("ErrorPage");
            
            var user = new User() {Id = uid};
            timetableMaker.MakeTimetable(user, algo, applicationFilters);
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