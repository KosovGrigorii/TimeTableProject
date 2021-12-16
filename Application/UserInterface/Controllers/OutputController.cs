using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TimetableApplication;

namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly IDictionary<string, OutputFormatter> outputFormatters;
        private readonly IUserData userToData;


        public OutputController(IEnumerable<OutputFormatter> outputFormatters, IUserData userToData)
        {
            this.outputFormatters = outputFormatters.ToDictionary(x => x.Extension);
            this.userToData = userToData;
        }

        [HttpGet]
        public IActionResult Index(string uid)
        {
            ViewBag.uid = uid;
            return View("Output");
        }

        public FileResult DownloadFile(string uid)
        {
            var extension = ".xlsx";
            var fileName = uid + extension;
            var path =  Path.Combine(Path.GetTempPath(), fileName);
            
            var formatter = outputFormatters[extension];
            formatter.GetOutputFile(path, userToData.GetTimeslots(uid));
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", Path.GetFileName(path));
        }
    }
}