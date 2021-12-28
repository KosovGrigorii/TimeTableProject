using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Infrastructure;
using TimetableApplication;
using UserInterface.Models;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        private readonly InputProvider inputProvider;
        private readonly App app;

        public MainPageController(InputProvider inputProvider, App app)
        {
            this.inputProvider = inputProvider;
            this.app = app;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FileFormUpload()
        {
            var uid = Guid.NewGuid().ToString();
            
            var fileInfo = Request.Form.Files[0];
            var strExtension = Path.GetExtension(fileInfo.FileName).Split('.').Last();
            var translated = Enum.TryParse<ParserExtension>(strExtension, out var extension);
            if (!translated)
            {
                return View("ErrorFileFormat");
                //ModelState.AddModelError("FileData", "Incorrect extension");
                //return RedirectToAction("Index");
                //return new ValidationResult("Incorrect extension");
            }

            var userInput = inputProvider.ParseInput(fileInfo, extension);
            app.SaveInput(uid, userInput.CourseSlots, userInput.TimeSchedule);
            
            return RedirectToAction("ToFiltersInput", new { uid = uid});
        }
        
        public IActionResult ToFiltersInput(string uid)
        {
            return RedirectToRoutePermanent("default", new
            {
                controller = "FiltersInput", action = "FiltersInput", uid = uid
            });
        }
    }
}