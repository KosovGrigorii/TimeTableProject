using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using TimetableApplication;
using TimetableDomain;
using UserInterface.Models;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        private readonly InputProvider inputProvider;
        private readonly TimetableMakingController timetableMaker;
        private readonly IUserData userToData;

        public MainPageController(IEnumerable<IInputParser> inputParsers,
            IEnumerable<ITimetableMaker> algorithms,
            IUserData userToData)
        {
        
            inputProvider = new InputProvider(inputParsers.ToDictionary(x => x.Extension));
            timetableMaker = new TimetableMakingController(algorithms.ToDictionary(x => x.Name));
            this.userToData = userToData;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FileFormUpload()
        {
            var uid = Guid.NewGuid().ToString();
            userToData.AddUser(uid);
            
            var fileInfo = Request.Form.Files[0];
            var strExtension = Path.GetExtension(fileInfo.FileName);
            var translated = Enum.TryParse<Parsers>(strExtension, out var extension);
            
            using (var stream = fileInfo.OpenReadStream())
            {
                var slots = inputProvider.ParseInput(stream, extension);
                userToData.SetInputInfo(uid, slots);
            }
            
            return RedirectToAction("FiltersInput", new { uid = uid});
        }

        public IActionResult FiltersInput(string uid)
        {
            var model = new UserID { ID = uid };
            return View(model);
        }
        
        public PartialViewResult GetFiltersInputField(string uid, string elementId)
        {
            var specifiedFilters = userToData.GetTeacherFilters(uid);
            var userFilters = new UserFilters() {Filters = specifiedFilters, Index = elementId};
            return PartialView("_SingleSpecifiedFilter", userFilters);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.Hours));
            
            timetableMaker.StartMakingTimeTable(uid, userToData, applicationFilters);
            return RedirectToAction("LoadingPage", new {uid = uid});
        }
        
        [HttpGet]
        public IActionResult LoadingPage(string uid)
        {
            return View("Loading", new UserID() {ID = uid});
        }
        
        [HttpPost]
        public IActionResult ToOutput(string uid)
        {
            return RedirectToRoutePermanent("default", new
            {
                controller = "Output", action = "Index", uid = uid
            });
        }
    }
}