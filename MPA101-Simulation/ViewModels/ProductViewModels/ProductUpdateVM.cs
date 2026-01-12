using System.ComponentModel.DataAnnotations;

namespace MPA101_Simulation.ViewModels.ProductViewModels;

public class ProductUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(256), MinLength(3)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(1024), MinLength(3)]
    public string Description { get; set; } = string.Empty;
    [Required, Range(0, 100000000)]
    public decimal Price { get; set; }
    [Required, Range(0, 5)]
    public int Rating { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public IFormFile? Image { get; set; }
}
