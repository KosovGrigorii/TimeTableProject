using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using TimetableApplication;
using TimetableCommonClasses;
using TimetableDomain;
using UserInterface.Models;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        private readonly Dictionary<string, IInputParser> inputParsers;
        private readonly Dictionary<string, ITimetableMaker> timetableMakers;

        public MainPageController(IEnumerable<IInputParser> inputParsers, 
            IEnumerable<ITimetableMaker> timetableMakers)
        {
            this.inputParsers = inputParsers.ToDictionary(x => x.Extension);
            this.timetableMakers = timetableMakers.ToDictionary(x => x.Name);
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
            
            UserToData.AddUser(uid);
            var slots = parser.ParseFile(stream);
            UserToData.SetInputInfo(uid, slots);
            return RedirectToAction("FiltersInput", new { uid = uid});
        }

        [HttpGet]
        public IActionResult FiltersInput(string uid)
        {
            ViewBag.uid = uid;
            return View("FiltersInput");
        }

        public PartialViewResult GetFiltersInputForm(string elementId)
        {
            var filterTypes = FilterInputHandler.GetFilterTypes();
            ViewBag.FilterTypes = new SelectList(filterTypes);
            ViewBag.Index = elementId;
            return PartialView("_SingleFilter");
        }
        
        [HttpPost]
        public PartialViewResult GetFiltersInputField(string uid, string filterKey, string elementId)
        {
            var specifiedFilters = FilterInputHandler.GetFiltersOfType(uid, filterKey);
            ViewBag.Index = elementId;
            return PartialView("_SingleSpecifiedFilter", specifiedFilters);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<Filter> filters, string uid)
        {
            //configurator.MakeTimetable(uid, filters);
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