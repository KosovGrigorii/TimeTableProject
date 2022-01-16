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
        private readonly InputRecipient appInputRecipient;

        public MainPageController(InputProvider inputProvider, InputRecipient appInputRecipient)
        {
            this.inputProvider = inputProvider;
            this.appInputRecipient = appInputRecipient; 
        }
        
        public ActionResult Index()
        {
            return View("Index", '.' + string.Join(", .", inputProvider.GetExtensions()));
        }

        [HttpPost]
        public IActionResult FileFormUpload()
        {
            var uid = Guid.NewGuid().ToString();
            
            var fileInfo = Request.Form.Files[0];
            var extension = Path.GetExtension(fileInfo.FileName).Split('.').Last();
            var availableExtension = inputProvider.IsExtensionAvailable(extension);
            if (!availableExtension)
                return View("ErrorFileFormat", '.' + string.Join(", .", inputProvider.GetExtensions()));
            var userInput = inputProvider.ParseInput(fileInfo.OpenReadStream(), extension);
            if (userInput.CourseSlots.Count() == 0)
            {
                throw new ArgumentException("No information about slots");
            }
            //if (userInput.TimeSchedule.LessonStarts == null || userInput.TimeSchedule.Duration == 0)
            //{
            //    throw new ArgumentException("No call schedule");
            //}
            appInputRecipient.SaveInput(new (uid), userInput.CourseSlots, userInput.TimeSchedule);
            
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