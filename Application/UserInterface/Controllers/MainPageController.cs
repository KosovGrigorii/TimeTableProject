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
        private readonly Dictionary<string, IInputParser> inputParsers;
        private readonly Dictionary<string, ITimetableMaker> timetableMakers;
        private readonly TimetableMakingController timetableMaker;
        private readonly IUserData userToData;

        public MainPageController(IEnumerable<IInputParser> inputParsers, 
            IEnumerable<ITimetableMaker> timetableMakerAlgorithms, 
            TimetableMakingController timetableMaker,
            IUserData userToData)
        {
            this.inputParsers = inputParsers.ToDictionary(x => x.Extension);
            this.timetableMakers = timetableMakerAlgorithms.ToDictionary(x => x.Name);
            this.timetableMaker = timetableMaker;
            this.userToData = userToData;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FileFormUpload()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var extension = Path.GetExtension(Request.Form.Files[0].FileName);
            var stream = Request.Form.Files[0].OpenReadStream();
            var uid = Guid.NewGuid().ToString();
            var parser = inputParsers[extension];
            
            userToData.AddUser(uid);
            var slots = parser.ParseFile(stream);
            userToData.SetInputInfo(uid, slots);
            return RedirectToAction("FiltersInput", new { uid = uid});
        }

        [HttpGet]
        public IActionResult FiltersInput(string uid)
        {
            ViewBag.uid = uid;
            return View("FiltersInput");
        }
        
        public PartialViewResult GetFiltersInputField(string uid, string elementId)
        {
            var specifiedFilters = userToData.GetTeacherFilters(uid);
            ViewBag.Index = elementId;
            return PartialView("_SingleSpecifiedFilter", specifiedFilters);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string uid)
        {
            var algorithm = timetableMakers["Genetic"];
            timetableMaker.StartMakingTimeTable(uid, algorithm, userToData, 
                filters.Select(x => new Filter(x.Name, x.Hours)));
            return RedirectToAction("LoadingPage", new {uid = uid});
        }
        
        [HttpGet]
        public IActionResult LoadingPage(string uid)
        {
            ViewBag.uid = uid;
            return View("Loading");
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