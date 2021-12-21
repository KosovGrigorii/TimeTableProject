using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimetableApplication;
using TimetableDomain;
using UserInterface.Models;
using Microsoft.EntityFrameworkCore;


namespace UserInterface
{
    public class MainPageController : Controller
    {
        private readonly InputProvider inputProvider;
        private readonly TimetableMakingProvider timetableMaker;
        private readonly IDatabaseClient databaseClient;

        public MainPageController(IEnumerable<IInputParser> inputParsers,
            IEnumerable<ITimetableMaker> algorithms,
            IEnumerable<IDatabaseClient> databaseClients)
        {
            inputProvider = new InputProvider(inputParsers.ToDictionary(x => x.Extension));
            timetableMaker = new TimetableMakingProvider(algorithms.ToDictionary(x => x.Name));
            databaseClient = databaseClients.Where(x => x.Name == DatabaseClient.Firebase).First();
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
            var strExtension = Path.GetExtension(fileInfo.FileName);
            var translated = Enum.TryParse<ParserExtension>(strExtension, out var extension);
            
            using (var stream = fileInfo.OpenReadStream())
            {
                var slots = inputProvider.ParseInput(stream, extension);
                databaseClient.SetInputInfo(uid, slots);
            }
            
            return RedirectToAction("FiltersInput", new { uid = uid});
        }

        public IActionResult FiltersInput(string uid)
        {
            var model = new UserID { ID = uid };
            return View(model);
        }
        
        public PartialViewResult _SingleSpecifiedFilter(string uid, string elementId)
        {
            var specifiedFilters = databaseClient.GetTeacherFilters(uid);
            var userFilters = new UserFilters() {Filters = specifiedFilters, Index = elementId};
            return PartialView(userFilters);
        }
        
        [HttpPost]
        public IActionResult GetFilters(IEnumerable<FilterUI> filters, string uid)
        {
            var applicationFilters = filters.Select(x => new Filter(x.Name, x.Hours));
            
            timetableMaker.StartMakingTimeTable(uid, databaseClient, applicationFilters);
            return RedirectToAction("LoadingPage", new {uid = uid});
        }
        
        [HttpGet]
        public IActionResult LoadingPage(string uid)
        {
            return View("Loading", new UserID() {ID = uid});
        }
        
        [HttpPost]
        public IActionResult ToOutput(string uid)
        {
            return RedirectToRoutePermanent("default", new
            {
                controller = "Output", action = "Index", uid = uid
            });
        }
    }
}