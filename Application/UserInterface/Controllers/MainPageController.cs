using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using TimetableApplication;



namespace UserInterface
{
    public class MainPageController : Controller
    {
        private readonly InputProvider inputProvider;
        private readonly InputExecutor appInputExecutor;
        private readonly IEnumerable<string> extensions;

        public MainPageController(InputProvider inputProvider, InputExecutor appInputExecutor)
        {
            this.inputProvider = inputProvider;
            this.appInputExecutor = appInputExecutor; 
            extensions = inputProvider.GetExtensions();
        }
        
        public ActionResult Index()
        {
            return View("Index", string.Join(", .", extensions));
        }

        [HttpPost]
        public IActionResult FileFormUpload()
        {
            var uid = Guid.NewGuid().ToString();
            var user = new User() {Id = uid};
            
            var fileInfo = Request.Form.Files[0];
            var extension = Path.GetExtension(fileInfo.FileName).Split('.').Last();
            var availableExtension = inputProvider.IsExtensionAvailable(extension);
            if (!availableExtension)
                return View("ErrorFileFormat", '.' + string.Join(", .", extensions));

            var userInput = inputProvider.ParseInput(fileInfo, extension);
            appInputExecutor.SaveInput(user, userInput.CourseSlots, userInput.TimeSchedule);
            
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