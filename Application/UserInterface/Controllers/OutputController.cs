using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;
using TimetableApplication;


namespace UserInterface
{
    public class OutputController: Controller
    {
        private readonly OutputProvider outputProvider;
        private readonly IDatabaseClient databaseClient;

        public OutputController(IEnumerable<OutputFormatter> outputFormatters, IDatabaseClient databaseClient)
        {
            outputProvider = new OutputProvider(outputFormatters.ToDictionary(x => x.Extension));
            this.databaseClient = databaseClient;
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
            var timeslots = databaseClient.GetTimeslots(uid);
            var filePath = outputProvider.GetPathToOutputFile(extension, uid, timeslots);
            
            var bytes = System.IO.File.ReadAllBytes(filePath);
            databaseClient.DeleteUserData(uid);
            return File(bytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}