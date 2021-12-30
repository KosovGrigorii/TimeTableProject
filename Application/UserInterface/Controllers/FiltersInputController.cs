using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimetableApplication;
using TimetableDomain;
using UserInterface.Models;

namespace UserInterface
{
    public class FiltersInputController : Controller
    {
        private readonly App app;
        private readonly IBackgroundTaskQueue taskQueue;
        private readonly CancellationToken cancellationToken;

        public FiltersInputController(App app, IBackgroundTaskQueue taskQueue)
        {
            this.app = app;
            this.taskQueue = taskQueue;
        }
        
        public IActionResult FiltersInput(string uid)
        {
            var model = new FiltersPageData() { Algorithms = new SelectList(app.GetAlgorithmNames()), UserID = uid };
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
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string algorithm, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.DaysCount, x.Days));
            var algoConverted = Enum.TryParse<Algorithm>(algorithm, out var algo);

            if (!algoConverted)
            {
                return View("ErrorPage");
            }

            app.AddUserWaitingForTimetable(uid);
            taskQueue.QueueBackgroundWorkItemAsync(
                async  (token) => {
                    if (!token.IsCancellationRequested)
                    {
                        try
                        {
                            app.MakeTimetable(uid, algo, applicationFilters);
                            await Task.CompletedTask;
                        }
                        catch (OperationCanceledException){ }
                    }
                });
            return View("LoadingPage", new UserID() { ID = uid });
        }
        
        [HttpPost]
        public IActionResult CheckCompleteness(string uid)
        {
            while (!app.IsMakingTimetableFinished(uid))
                Thread.Sleep(300);

            return RedirectToRoutePermanent("default", new
            {
                controller = "Output", action = "Index", uid = uid
            });
        }
    }
}