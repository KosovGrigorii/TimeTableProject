using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class MainPageController : Controller
    {
        public ActionResult Index()  => View();
        
        [HttpPost]
        public ActionResult GetEntitiesInfo(List<StudyHoursInfo> model) => Connect();

        public ActionResult Connect() => View();
    }
}