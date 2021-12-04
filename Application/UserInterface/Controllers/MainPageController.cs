using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using TimetableApplication;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Linq;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        public ActionResult Index()  => View();

        [HttpPost]
        public IActionResult GetExcelInput()
        {
            // InputHandler.ParseInput(input, inputFormat)
            return FiltersInput();
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
            //MakeTimeTable(filters);
                //Асинхронно?
            return View("Loading");
        }
        
        /*[HttpPost]
        public IActionResult ToOutput()
        {
            return RedirectToRoute("default", new
            {
                controller = "Output", action = "Index"
            });
        }*/

        private void MakeTimeTable(IEnumerable<Filter> filters)
        {
            var timetableMaker = new TimetableMakingController();
            timetableMaker.StartMakingTimeTable(filters);
        }

        // [HttpPost]
        // public void PostOutputFormat(string format)
        // {
        //     //Return partial view with file.
        // }
        
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