using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimetableApplication;

namespace UserInterface
{
    public class MainPageController : Controller
    {
        public ActionResult Index()  => View();

        [HttpGet]
        public IActionResult FiltersInput()
        {
            var filterTypes = new List<string>
            {
                "Whole university", 
                "Teacher", 
                "Group"
            };
            ViewBag.FilterTypes = new SelectList(filterTypes);
            return View();
        }

        [HttpPost]
        public PartialViewResult GetFiltersInputField(string filterKey)
        {
            var typeToFilter = new Dictionary<string, List<string>>
            {
                {"Teacher", new List<string>{"T1", "T2", "T3"}},
                {"Group", new List<string>{"First", "Second", "Third"}}
            };
            return PartialView("_SingleSpecifiedFilter", typeToFilter[filterKey]);
        }

        //[HttpPost]
        public string GetHoursInfo() //input
        {
            var inputTime = new byte[] {};
            var format = ".sthg";
            
            //Ввод списка аудиторий
            //Ввод расписания звонков
            //Ввод учебных часов
            //var inputHandler = new InputHandler();
            InputHandler.ParseInput(inputTime, format);
            
            return "View2 - filters"; //View2()
        }

        public string GetFilters()
        {
            FilterChecker.GetFilterInfo();
            FilterChecker.AddFilters(new List<Filter>());
            return "View3 - process";
        }

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