using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TimetableDomain;
using UserInterface.Models;

namespace UserInterface
{
    public class OutputController: Controller
    {
        [HttpGet]
        public IActionResult Index(string uid)
        {
            ViewBag.uid = uid;
            return View("Output");
        }

        public FileResult DownloadFile(string uid)
        {
            var filePath = ApplicationConfigurator.Configurator.GetOutputFile(uid, ".xlsx");
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}