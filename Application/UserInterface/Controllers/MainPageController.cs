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
            ViewBag.FilterTypres = new SelectList(filterTypes);
            return View();
        }
        
        [HttpPost]
        public IActionResult OnFilterTypeInput(string filterType)
        {
            var typeToFilter = new Dictionary<string, SelectList>
            {
                {"Teacher", new SelectList(new[] {"T1", "T2", "T3"})},
                {"Group", new SelectList(new[] {"First", "Second", "Third"})}
            };
            return PartialView("FiltersInput", typeToFilter[filterType]);
        }

        [HttpGet]
        public void GetSpecifiedFilters(string filterType)
        {
            ViewBag.Filters = new SelectList(new [] {"One", "Oopsie", "Outro"});
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