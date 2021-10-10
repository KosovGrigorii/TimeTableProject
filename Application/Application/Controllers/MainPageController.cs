using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class MainPageController : Controller
    {
        public ActionResult Index()  => View();
        
        [HttpPost]
        public string GetHoursInfo(List<StudyHoursInfo> model) => "Oтправлено";
        
    }
}