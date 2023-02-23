using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.ViewModels.Account;

public class RegisterUserViewModel
{
    [Required(ErrorMessage = "E-post saknas")]
    [DisplayName("E-Post")]
    [EmailAddress(ErrorMessage = "Felaktig inmatning av e-postadress")]
    // Email representerar Username och Epost 
    public string Email { get; set; }

    [Required(ErrorMessage = "Lösenord saknas")]
    [DisplayName("Lösenord")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Bekräfta lösenord saknas")]
    [DisplayName("Bekräfta lösenord")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Lösenord och bekräfta lösenord matchar ej!")]
    public string ConfirmPassword { get; set; }
}
