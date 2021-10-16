using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Application.Controllers
{
    public class MainPageController : Controller
    {
        private IHostEnvironment environment;
        private string filesDir;
        
        public MainPageController(IHostEnvironment env)
        {
            environment = env;
            filesDir = Path.Combine(environment.ContentRootPath, "FilesStorage");
        }
        
        public ActionResult Index()  => View();

        public ActionResult GetEntitiesInfo(IFormFile hoursFile)
        {
            using (var filestream = new FileStream(Path.Combine(filesDir, hoursFile.FileName), FileMode.Create, FileAccess.Write))
            {
                hoursFile.CopyTo(filestream);
            }
            return RedirectToAction("Filters");
        }

        public ActionResult Filters() => View();
    }
}