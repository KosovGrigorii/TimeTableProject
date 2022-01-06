using System;
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
        private readonly InputExecutor appInputExecutor;
        private readonly PageAcceptedExtensions extensions;

        public MainPageController(InputProvider inputProvider, InputExecutor appInputExecutor)
        {
            this.inputProvider = inputProvider;
            this.appInputExecutor = appInputExecutor;
            extensions = new PageAcceptedExtensions(inputProvider.GetExtensions());
        }
        
        public ActionResult Index()
        {
            return View(extensions);
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
                return View("ErrorFileFormat", extensions);

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