using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TimetableApplication;

namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly Configurator configurator;


        public OutputController(Configurator configurator)
        {
            this.configurator = configurator;
        }

        [HttpGet]
        public IActionResult Index(string uid)
        {
            ViewBag.uid = uid;
            return View("Output");
        }

        public FileResult DownloadFile(string uid)
        {
            var filePath = configurator.GetOutputFile(uid, ".xlsx");
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}