using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Castle.Core.Internal;
using TimetableCommonClasses;
using UserInterface.Models;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        public ActionResult Index(string user)
        {
            Console.WriteLine(user);
            return View(user);
        }

        [HttpPost]
        public IActionResult FileFormUpload()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var extension = Path.GetExtension(Request.Form.Files[0].FileName);
            var stream = Request.Form.Files[0].OpenReadStream();
            
            ApplicationConfigurator.AppConfigurator.Input(stream, extension);
            return RedirectToAction("FiltersInput");
        }

        [HttpGet]
        public IActionResult FiltersInput()
        {
            return View("FiltersInput");
        }

        public PartialViewResult GetFiltersInputForm(string elementId)
        {
            var filterTypes = ApplicationConfigurator.AppConfigurator.GetFilterTypes();
            ViewBag.FilterTypes = new SelectList(filterTypes);
            ViewBag.Index = elementId;
            return PartialView("_SingleFilter");
        }

        [HttpPost]
        public PartialViewResult GetFiltersInputField(string filterKey, string elementId)
        {
            var specifiedFilters = ApplicationConfigurator.AppConfigurator.GetFiltersOfType(filterKey);
            ViewBag.Index = elementId;
            return PartialView("_SingleSpecifiedFilter", specifiedFilters);
        }

        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters)
        {
            ApplicationConfigurator.AppConfigurator.MakeTimetable(filters
                .Select(x => ApplicationConfigurator.AppConfigurator.GetFilter(x.Category, x.Name, x.Hours)));
            return View("Loading");
        }
        
        [HttpPost]
        public IActionResult ToOutput()
        {
            return RedirectToRoutePermanent("default", new
            {
                controller = "Output", action = "Index"
            });
        }
    }
}