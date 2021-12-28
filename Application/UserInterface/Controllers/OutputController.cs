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
            var extensions = new SelectList(app.GetOutputExtensions());
            return View("Output", new OutputPageData()
            {
                OutputExtensions = extensions,
                UserId = uid
            });
        }

        public FileResult DownloadFile(string extension, string uid)
        {
            var translated = Enum.TryParse<OutputExtension>(extension, out var outputExtension);

            var fileByteArray = app.GetOutput(uid, outputExtension);
            return File(fileByteArray, "application/octet-stream", $"Timetable.{extension.ToLower()}");
        }
    }
}