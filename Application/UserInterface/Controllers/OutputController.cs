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
        private readonly IWebHostEnvironment Environment;
        private IEnumerable<TimeSlot> TimeSlots => TimetableApplication.DB.Timeslots;

        public OutputController(IWebHostEnvironment environment)
        {
            Environment = environment;
        }
        
        [HttpGet]
        public IActionResult Index() => View("Output");

        public FileResult DownloadFile()
        {
            var path = Path.Combine(Environment.ContentRootPath, "Files", "output.xlsx");
            var file = ApplicationConfigurator.Configurator.GetOutputFile(".xlsx", path, TimeSlots);
            //var path = file.FullName;
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", file.Name);
        }
    }
}