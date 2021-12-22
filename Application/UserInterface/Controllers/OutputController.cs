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

        public OutputController(IEnumerable<OutputFormatter> outputFormatters, 
            IEnumerable<IDatabaseWrapper<string, DatabaseSlot>> slotWrappers,
            IEnumerable<IDatabaseWrapper<string, DatabaseTimeslot>> timeslotWrappers)
        {
            outputProvider = new OutputProvider(outputFormatters.ToDictionary(x => x.Extension));
            databaseProvider = new DatabaseProvider(
                slotWrappers.ToDictionary(x => x.BaseName), 
                timeslotWrappers.ToDictionary(x => x.BaseName));
        }

        [HttpGet]
        public IActionResult Index(string uid)
        {
            return View("Output", new UserID() {ID = uid});
        }

        public FileResult DownloadFile(string uid)
        {
            var strExtension = ".xlsx";
            var translated = Enum.TryParse<OutputExtension>(strExtension, out var extension);
            var timeslots = databaseProvider.GetTimeslots(uid);
            var filePath = outputProvider.GetPathToOutputFile(extension, uid, timeslots);
            
            var bytes = System.IO.File.ReadAllBytes(filePath);
            databaseProvider.DeleteUserData(uid);
            return File(bytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}