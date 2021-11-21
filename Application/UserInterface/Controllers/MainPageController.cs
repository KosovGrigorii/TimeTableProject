using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TimetableApplication;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Filter = TimetableApplication.Filter;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        public ActionResult Index()  => View();

        [HttpPost]
        public IActionResult FileFormUpload(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var extension = Path.GetExtension(file.FileName);
            var stream = new MemoryStream();
            file.CopyTo(stream);
            InputHandler.ParseInput(stream, extension);
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
            MakeTimeTable(filters);
                //Асинхронно?
            return View("Loading");
        }

        private void MakeTimeTable(IEnumerable<Filter> filters)
        {
            var timetableMaker = new TimetableMakingController();
            timetableMaker.StartMakingTimeTable(filters);
        }

        [HttpPost]
        public void PostOutputFormat(string format)
        {
            //Return partial view with file.
        }
    }
}