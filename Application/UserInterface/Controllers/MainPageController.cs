using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TimetableApplication;

namespace PreviousVersion.Controllers
{
    public class MainPageController : Controller
    {
        public ActionResult Index()  => View();

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