using Microsoft.AspNetCore.Mvc;

namespace MPA101_Simulation.Controllers
{
    public class HomeController : Controller
    {
       public IActionResult Index()
        {
            return View();
        }
    }
}
