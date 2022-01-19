using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using TimetableApplication;
using UserInterface.Models;

namespace UserInterface
{
    public class FiltersInputController : Controller
    {
        private readonly FiltersPageInterface appInterface;
        private readonly ImplementationSelector<FilterGetterParameters, FilterPartialViewData, IImplementation<FilterGetterParameters, FilterPartialViewData>> filtersInputProvider;

        public FiltersInputController(
            FiltersPageInterface appInterface, 
            ImplementationSelector<FilterGetterParameters, FilterPartialViewData, IImplementation<FilterGetterParameters, FilterPartialViewData>> filtersInputProvider)
        {
            this.appInterface = appInterface;
            this.filtersInputProvider = filtersInputProvider;
        }
        
        public IActionResult FiltersInput(string uid)
        {
            var model = new FiltersPageData() { Algorithms = new (appInterface.GetAlgorithmNames()), UserID = uid };
            return View(model);
        }
        
        public PartialViewResult _FilterChoosingForm(string userId, string elementId)
        {
            var model = new FilterChoose 
            { 
                Categories = new (filtersInputProvider.GetTypes()), 
                UserId = userId, 
                FilterId = elementId
            };
            return PartialView(model);
        }

        public PartialViewResult ChooseSingleFilter(string filterName, string userId, string elementId)
        {
            var specifiedFilters = appInterface.GetTeachersNameForFilters(new (userId));
            var (partialViewName, model) = filtersInputProvider.GetResult(filterName, new(specifiedFilters, elementId));
            return PartialView(partialViewName, model);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string algorithm, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.DaysCount, x.Days));
            var algoAvailable = appInterface.GetAlgorithmNames().Contains(algorithm);
            if (!algoAvailable)
                return View("ErrorPage");
            appInterface.AddTimetableMakingTask(new (uid), algorithm, applicationFilters);
            return RedirectToAction("ToOutputPage", new { uid = uid });
        }
        
        public IActionResult ToOutputPage(string uid)
        {
            return RedirectToRoutePermanent("default", new
            {
                controller = "Output", action = "Index", uid = uid
            });
        }
    }
}