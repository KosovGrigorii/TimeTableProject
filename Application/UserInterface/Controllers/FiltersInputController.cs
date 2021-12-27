using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimetableApplication;
using UserInterface.Models;

namespace UserInterface
{
    public class FiltersInputController : Controller
    {
        private readonly App app;

        public FiltersInputController(App app)
        {
            this.app = app;
        }
        
        public IActionResult FiltersInput(string uid)
        {
            var model = new UserID { ID = uid };
            return View(model);
        }
        
        public PartialViewResult _FilterChoosingForm(string userId, string elementId)
        {
            var model = new FilterChoose {UserId = userId, FilterId = elementId};
            return PartialView(model);
        }

        public PartialViewResult ChooseSingleFilter(string filterName, string userId, string elementId)
        {
            var specifiedFilters = app.GetTeachers(userId);
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
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.DaysCount, x.Days));
            app.MakeTimetable(uid, applicationFilters);
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