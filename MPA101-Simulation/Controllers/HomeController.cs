using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPA101_Simulation.Contexts;
using MPA101_Simulation.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace MPA101_Simulation.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImagePath = x.ImagePath,
                CategoryName = x.Category.Name,
                Price = x.Price,
                Rating = x.Rating
            }).ToListAsync();

            //ctrl+m+g goto view || go to action

            return View(products);
        }

        [Authorize(Roles = "Member")]
        public IActionResult Test()
        {
            return Ok("Salam");
        }


    }
}
