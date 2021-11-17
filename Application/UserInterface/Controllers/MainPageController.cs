using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimetableApplication;

namespace UserInterface
{
    public class MainPageController : Controller
    {
        public ActionResult Index()  => View();
        
        public IActionResult GetHoursInfo() //input
        {
            var inputTime = new byte[] {};
            var format = ".sthg";
            
            //Ввод списка аудиторий
            //Ввод расписания звонков
            //Ввод учебных часов
            //var inputHandler = new InputHandler();
            InputHandler.ParseInput(inputTime, format);
            
            return FiltersInput();
        }

        [HttpGet]
        public IActionResult FiltersInput()
        {
            return View();
        }

        public PartialViewResult GetFiltersInputForm(string elementId)
        {
            var filterTypes = new List<string>
            {
                "Whole university", 
                "Teacher", 
                "Group"
            };
            ViewBag.FilterTypes = new SelectList(filterTypes);
            ViewBag.Index = elementId;
            return PartialView("_SingleFilter");
        }

        [HttpPost]
        public PartialViewResult GetFiltersInputField(string filterKey, string elementId)
        {
            var typeToFilter = new Dictionary<string, List<string>>
            {
                {"Teacher", new List<string>{"T1", "T2", "T3"}},
                {"Group", new List<string>{"First", "Second", "Third"}}
            };
            ViewBag.Index = elementId;
            return PartialView("_SingleSpecifiedFilter", typeToFilter[filterKey]);
        }
        
        [HttpPost]
        public string GetFilters(IEnumerable<Filter> filters) => "Oтправлено";

        public void StartMakingTimetable()
        {
            var maker = new TimetableMakingController();
            maker.GetEntitiesFromDB();
            maker.StartMalingTimeTable();
        }

        public string GetResultTimetable()
        {
            return "View4 - result"; //файл из хранилища
        }
    }
}