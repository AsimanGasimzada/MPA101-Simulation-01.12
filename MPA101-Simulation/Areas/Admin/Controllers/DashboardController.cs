using Microsoft.AspNetCore.Mvc;

namespace MPA101_Simulation.Areas.Admin.Controllers;
[Area("Admin")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}
