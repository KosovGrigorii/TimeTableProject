using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TimetableApplication;

namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly Configurator configurator;
        private readonly IWebHostEnvironment Environment;
        

        public OutputController(Configurator configurator, IWebHostEnvironment environment)
        {
            this.configurator = configurator;
            Environment = environment;
        }
        
        [HttpGet]
        public IActionResult Index() => View("Output");

        public FileResult DownloadFile()
        {
            var path = Path.Combine(Environment.ContentRootPath, "Files", "output.xlsx");
            var file = configurator.GetOutputFile(".xlsx", path);
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", file.Name);
        }
    }
}