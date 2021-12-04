using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly IWebHostEnvironment Environment;

        public OutputController(IWebHostEnvironment environment)
        {
            Environment = environment;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            //Fetch all files in the Folder (Directory).
            var filePaths = Directory.GetFiles(Path.Combine(Environment.ContentRootPath, "Files/"));
 
            //Copy File names to Model collection.
            var files = filePaths.Select(filePath
                => new FileModel { FileName = Path.GetFileName(filePath) }).ToList();
            Thread.Sleep(5000);

            return View("Output", files);
        }
        
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            var path = Path.Combine(Environment.ContentRootPath, "Files/") + fileName;
 
            //Read the File data into Byte Array.
            var bytes = System.IO.File.ReadAllBytes(path);
 
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
    
    public class FileModel
    {
        public string FileName { get; set; }
    }
}