using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserInterface.Models;
using TimetableApplication;


namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly App app;

        public OutputController(App app)
        {
            this.app = app;
        }

        [HttpGet]
        public IActionResult Index(string uid)
        {
            var extensions = new SelectList(Enum.GetValues(typeof(OutputExtension)));
            ViewBag.Extensions = extensions;
            return View("Output", new UserID {ID = uid});
        }

        public FileResult DownloadFile(string extension, string uid)
        {
            var translated = Enum.TryParse<OutputExtension>(extension, out var outputExtension);

            var fileStream = app.GetOutput(uid, outputExtension);
            return File(fileStream, "application/octet-stream", $"Timetable.{extension.ToLower()}");
        }
    }
}