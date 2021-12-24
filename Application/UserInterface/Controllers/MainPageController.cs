using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Infrastructure;
using TimetableApplication;
using UserInterface.Models;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        private readonly InputProvider inputProvider;
        private readonly DatabaseProvider databaseProvider;

        public MainPageController(InputProvider inputProvider,
            DatabaseProvider databaseProvider)
        {
            this.inputProvider = inputProvider;
            this.databaseProvider = databaseProvider;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FileFormUpload()
        {
            var uid = Guid.NewGuid().ToString();
            
            var fileInfo = Request.Form.Files[0];
            var strExtension = Path.GetExtension(fileInfo.FileName).Split('.').Last();
            var translated = Enum.TryParse<ParserExtension>(strExtension, out var extension);
            
            using (var stream = fileInfo.OpenReadStream())
            {
                var slots = inputProvider.ParseInput(stream, extension);
                databaseProvider.AddInputSlotInfo(uid, slots);
            }
            
            return RedirectToAction("ToFiltersInput", new { uid = uid});
        }
        
        public IActionResult ToFiltersInput(string uid)
        {
            return RedirectToRoutePermanent("default", new
            {
                controller = "FiltersInput", action = "FiltersInput", uid = uid
            });
        }
    }
}