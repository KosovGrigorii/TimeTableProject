using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using TimetableApplication;
using TimetableDomain;


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
            return View("Output", new UserID() {ID = uid});
        }

        public FileResult DownloadFile(string uid)
        {
            var strExtension = ".pdf";
            var translated = Enum.TryParse<OutputExtension>(strExtension, out var extension);
            var timeslots = databaseProvider.GetTimeslots(uid);
            
            var bytes =  outputProvider.GetPathToOutputFile(extension, uid, timeslots);
            databaseProvider.DeleteUserData(uid);
            return File(bytes, "application/octet-stream", "Timetable.xlsx");
        }
    }
}