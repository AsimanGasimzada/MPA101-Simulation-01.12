using System.ComponentModel.DataAnnotations;

namespace MPA101_Simulation.ViewModels.UserViewModels;

public class LoginVM
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
   
    [Required, MaxLength(256), MinLength(6), DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
   
}


//<script>
//
//if(Password.Length>256){
//throw Exception(""
//}
//
//</script>