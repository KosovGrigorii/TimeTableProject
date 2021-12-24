using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserInterface.Models;

namespace UserInterface
{
    public class LoadingPageController : Controller
    {
        [HttpGet]
        public IActionResult Index(string uid)
        {
            return View("Index", new UserID() { ID = uid });
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