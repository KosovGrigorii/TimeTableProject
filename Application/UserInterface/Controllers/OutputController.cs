using System.Linq;
using System.Threading;
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
        
        public ActionResult CheckCompleteness(string uid)
        {
            return Ok(new { isCompleted = outputExecutor.IsTimetableReadyFor(new (){Id = uid}) });
        }

        public FileResult DownloadFile(string extension, string uid)
        {
            var translated = outputExecutor.GetOutputExtensions().Contains(extension);
            if (!translated)
                RedirectToAction("ErrorAction", uid);

            var user = new User() {Id = uid};
            var fileByteArray = outputExecutor.GetOutput(user, extension);
            return File(fileByteArray, "application/octet-stream", $"Timetable.{extension.ToLower()}");
        }

        public IActionResult ErrorAction(string uid)
        {
            return View(new UserID(){ID = uid});
        }
    }
}