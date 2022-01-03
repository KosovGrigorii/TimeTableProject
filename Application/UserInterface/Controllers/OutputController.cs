using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserInterface.Models;
using TimetableApplication;


namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly OutputExecutor outputExecutor;

        public OutputController(OutputExecutor outputExecutor)
        {
            this.outputExecutor = outputExecutor;
        }

        [HttpGet]
        public IActionResult Index(string uid)
        {
            var extensions = new SelectList(outputExecutor.GetOutputExtensions());
            return View("Output", new OutputPageData()
            {
                OutputExtensions = extensions,
                UserID = uid
            });
        }

        public FileResult DownloadFile(string extension, string uid)
        {
            var translated = Enum.TryParse<OutputExtension>(extension, out var outputExtension);

            if (!translated)
            {
                RedirectToAction("ErrorAction", uid);
            }

            var user = new User() {Id = uid};
            var fileByteArray = outputExecutor.GetOutput(user, outputExtension);
            return File(fileByteArray, "application/octet-stream", $"Timetable.{extension.ToLower()}");
        }

        public IActionResult ErrorAction(string uid)
        {
            return View(new UserID(){ID = uid});
        }
    }
}