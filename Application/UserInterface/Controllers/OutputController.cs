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
        private readonly IUserData userToData;

        public OutputController(IEnumerable<OutputFormatter> outputFormatters, IUserData userToData)
        {
            outputProvider = new OutputProvider(outputFormatters.ToDictionary(x => x.Extension));
            this.userToData = userToData;
        }

        [HttpGet]
        public IActionResult Index(string uid)
        {
            return View("Output", new UserID() {ID = uid});
        }

        public FileResult DownloadFile(string uid)
        {
            var strExtension = ".xlsx";
            var translated = Enum.TryParse<Formatters>(strExtension, out var extension);
            var timeslots = userToData.GetTimeslots(uid);
            var filePath = outputProvider.GetOutputPath(extension, uid, timeslots);
            
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/octet-stream", Path.GetFileName(filePath));
        }
    }
}