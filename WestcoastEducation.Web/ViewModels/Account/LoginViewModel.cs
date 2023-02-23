using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Användarnamn saknas")]
    [DisplayName("Användarnamn/E-Post")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Lösenord saknas")]
    [DisplayName("Lösenord")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
