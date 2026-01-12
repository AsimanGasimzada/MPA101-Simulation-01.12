using Microsoft.EntityFrameworkCore;
using MPA101_Simulation.Configurations;
using MPA101_Simulation.Models;
using System.Reflection;

namespace MPA101_Simulation.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //modelBuilder.ApplyConfiguration<ProductConfiguration>()
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

}
