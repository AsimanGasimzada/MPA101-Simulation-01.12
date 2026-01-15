using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MPA101_Simulation.Contexts;
using MPA101_Simulation.Helpers;
using MPA101_Simulation.Models;
using System.Threading.Tasks;

namespace MPA101_Simulation
{
    public class Program(DbContextInitalizer initalizer)
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<DbContextInitalizer>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });


            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var contextInitalizer = scope.ServiceProvider.GetRequiredService<DbContextInitalizer>();

            await contextInitalizer.InitializeDatabaseAsync();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
