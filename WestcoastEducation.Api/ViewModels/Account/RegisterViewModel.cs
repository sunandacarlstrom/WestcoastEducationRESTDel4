using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels.Account;
public class RegisterViewModel : LoginViewModel
{
    // [Required(ErrorMessage = "E-post saknas")]
    // [DisplayName("E-Post")]
    // [EmailAddress(ErrorMessage = "Felaktig inmatning av e-postadress")]
    public string Email { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    // [Required(ErrorMessage = "Bekräfta lösenord saknas")]
    // [DisplayName("Bekräfta lösenord")]
    // [DataType(DataType.Password)]
    // [Compare("Password", ErrorMessage = "Lösenord och bekräfta lösenord matchar ej!")]
    // public string ConfirmPassword { get; set; }
}
