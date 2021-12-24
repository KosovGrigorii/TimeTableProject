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
        private readonly TimetableMakingProvider timetableMaker;
        private readonly DatabaseProvider databaseProvider;

        public FiltersInputController(TimetableMakingProvider timetableMaker, DatabaseProvider databaseProvider)
        {
            this.timetableMaker = timetableMaker;
            this.databaseProvider = databaseProvider;
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
            return _SingleSpecifiedFilter(userId, elementId);
        }
        
        public PartialViewResult _SingleSpecifiedFilter(string uid, string elementId)
        {
            var specifiedFilters = databaseProvider.GetTeacherFilters(uid);
            var userFilters = new UserFilters() {Filters = specifiedFilters, Index = elementId};
            return PartialView(userFilters);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.Days));

            timetableMaker.StartMakingTimeTable(uid, databaseProvider, applicationFilters);
            var timetableTask = new Task(() => timetableMaker.StartMakingTimeTable(uid, databaseProvider, applicationFilters));
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