using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPA101_Simulation.Contexts;
using MPA101_Simulation.Helpers;
using MPA101_Simulation.Models;
using MPA101_Simulation.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace MPA101_Simulation.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _folderPath;


    public ProductController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
    }

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

        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        await _sendCategoriesWithViewBag();

        return View();
    }


    [HttpPost]

    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        await _sendCategoriesWithViewBag();

        if (!ModelState.IsValid)
            return View(vm);

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId", "This category is not found");
            return View(vm);
        }


        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Image's maximum size must be 2 mb");
            return View(vm);
        }


        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("Image", "You can upload file in only image format");
            return View(vm);
        }

        //string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;


        //string path = Path.Combine(_folderPath, uniqueFileName);


        //using FileStream stream = new(path, FileMode.Create);

        //await vm.Image.CopyToAsync(stream)
        //
        //;


        string uniqueFileName = await vm.Image.FileUploadAsync(_folderPath);




        Product product = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            Rating = vm.Rating,
            Price = vm.Price,
            CategoryId = vm.CategoryId,
            ImagePath = uniqueFileName
        };


        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));




    }


    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();


        string deletedImagePath = Path.Combine(_folderPath, product.ImagePath);

        //if (System.IO.File.Exists(deletedImagePath))
        //    System.IO.File.Delete(deletedImagePath);

        FileHelper.FileDelete(deletedImagePath);

        return RedirectToAction("Index");

    }



    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        ProductUpdateVM vm = new()
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Rating = product.Rating
        };

        await _sendCategoriesWithViewBag();

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateVM vm)
    {
        await _sendCategoriesWithViewBag();

        if (!ModelState.IsValid)
            return View(vm);

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId", "This category is not found");
            return View(vm);
        }


        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Image's maximum size must be 2 mb");
            return View(vm);
        }


        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("Image", "You can upload file in only image format");
            return View(vm);
        }


        var existProduct = await _context.Products.FindAsync(vm.Id);

        if (existProduct is null)
            return BadRequest();

        existProduct.Name = vm.Name;
        existProduct.Description = vm.Description;
        existProduct.Rating = vm.Rating;
        existProduct.CategoryId = vm.CategoryId;
        existProduct.Price = vm.Price;


        if (vm.Image is { })
        {
            string newImagePath = await vm.Image.FileUploadAsync(_folderPath);

            string oldImagePath = Path.Combine(_folderPath, existProduct.ImagePath);
            FileHelper.FileDelete(oldImagePath);

            existProduct.ImagePath = newImagePath;
        }


        _context.Products.Update(existProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private async Task _sendCategoriesWithViewBag()
    {
        var categories = await _context.Categories.Select(c => new SelectListItem()
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToListAsync();

        ViewBag.Categories = categories;
    }

}

//ctrl+m+o  collapse all methods