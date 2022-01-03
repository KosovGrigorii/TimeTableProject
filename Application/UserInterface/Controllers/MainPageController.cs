using System;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using TimetableApplication;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        private readonly InputProvider inputProvider;
        private readonly InputExecutor inputExecutor;

        public MainPageController(InputProvider inputProvider, InputExecutor inputExecutor)
        {
            this.inputProvider = inputProvider;
            this.inputExecutor = inputExecutor;
        }
        
        public ActionResult Index()
        {
            //InputParsers 
            return View();
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
                return View("ErrorFileFormat");

            var userInput = inputProvider.ParseInput(fileInfo, extension);
            inputExecutor.SaveInput(user, userInput.CourseSlots, userInput.TimeSchedule);
            
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