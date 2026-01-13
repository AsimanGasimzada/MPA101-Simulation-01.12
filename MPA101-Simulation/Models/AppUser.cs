using Microsoft.AspNetCore.Identity;

namespace MPA101_Simulation.Models;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } = string.Empty;
}
