using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserInterface.Models;
using TimetableApplication;


namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly OutputProvider outputProvider;
        private readonly DatabaseProvider databaseProvider;

        public OutputController( OutputProvider outputProvider, DatabaseProvider databaseProvider)
        {
            this.outputProvider = outputProvider;
            this.databaseProvider = databaseProvider;
        }

        [HttpGet]
        public IActionResult Index(string uid)
        {
            var extensions = new SelectList(Enum.GetValues(typeof(OutputExtension)));
            ViewBag.Extensions = extensions;
            return View("Output", new UserID {ID = uid});
        }

        public FileResult DownloadFile(string extension, string uid)
        {
            var translated = Enum.TryParse<OutputExtension>(extension, out var outputExtension);
            var timeslots = databaseProvider.GetTimeslots(uid);
            
            var bytes =  outputProvider.GetPathToOutputFile(outputExtension, uid, timeslots);
            databaseProvider.DeleteUserData(uid);
            return File(bytes, "application/octet-stream", $"Timetable.{extension.ToLower()}");
        }
    }
}