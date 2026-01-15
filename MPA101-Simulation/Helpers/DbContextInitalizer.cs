using Microsoft.AspNetCore.Identity;
using MPA101_Simulation.Enums;
using MPA101_Simulation.Models;
using MPA101_Simulation.ViewModels.UserViewModels;

namespace MPA101_Simulation.Helpers;

public class DbContextInitalizer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AdminVM _admin;

    public DbContextInitalizer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;

        _admin = _configuration.GetSection("AdminSettings").Get<AdminVM>() ?? new();
    }

    public async Task InitializeDatabaseAsync()
    {
        await CreateRoles();

        await CreateAdmin();

    }

    private async Task CreateAdmin()
    {
        AppUser user = new()
        {
            UserName = _admin.UserName,
            Email = _admin.Email,
            Fullname = _admin.Fullname
        };


        var result = await _userManager.CreateAsync(user, _admin.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, IdentityRoles.Admin.ToString());
        }
    }

    private async Task CreateRoles()
    {
        //await _roleManager.CreateAsync(new()
        //{
        //    Name = "Member"
        //});

        //await _roleManager.CreateAsync(new()
        //{
        //    Name = "Admin"
        //});


        foreach (var role in Enum.GetNames(typeof(IdentityRoles)))
        {
            await _roleManager.CreateAsync(new()
            {
                Name = role
            });
        }
    }
}
