using MPA101_Simulation.Models.Common;

namespace MPA101_Simulation.Models;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Rating { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
