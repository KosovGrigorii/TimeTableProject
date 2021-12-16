using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using TimetableApplication;
using System.IO;
using Castle.Core.Internal;
using UserInterface.Models;
using Filter = TimetableApplication.Filter;


namespace UserInterface
{
    public class MainPageController : Controller
    {
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
            
            ApplicationConfigurator.Configurator.Input(uid, stream, extension);
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
        public PartialViewResult GetFiltersInputField(string filterKey, string elementId)
        {
            var specifiedFilters = FilterInputHandler.GetFiltersOfType(filterKey);
            ViewBag.Index = elementId;
            return PartialView("_SingleSpecifiedFilter", specifiedFilters);
        }

        [HttpPost]
        public IActionResult GetFilters(IEnumerable<Filter> filters, string uid)
        {
            ApplicationConfigurator.Configurator.MakeTimetable(uid, filters);
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