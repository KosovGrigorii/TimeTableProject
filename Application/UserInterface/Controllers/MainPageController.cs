using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using TimetableApplication;
using System.IO;
using UserInterface.Models;
using Filter = TimetableApplication.Filter;


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
            
            ApplicationConfigurator.Configurator.Input(stream, extension);
            return RedirectToAction("FiltersInput");
        }

        [HttpGet]
        public IActionResult FiltersInput()
        {
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
        public PartialViewResult GetFiltersInputField(string filterKey, string elementId)
        {
            var specifiedFilters = FilterInputHandler.GetFiltersOfType(filterKey);
            ViewBag.Index = elementId;
            return PartialView("_SingleSpecifiedFilter", specifiedFilters);
        }

        [HttpPost]
        public IActionResult GetFilters(IEnumerable<Filter> filters)
        {
            ApplicationConfigurator.Configurator.MakeTimetable(filters);
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