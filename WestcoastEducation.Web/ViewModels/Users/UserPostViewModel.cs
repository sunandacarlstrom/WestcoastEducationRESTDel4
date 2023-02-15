using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.ViewModels.Users;

public class UserPostViewModel
{
    [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
    [DisplayName("Användarnamn")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "E-postadress är obligatoriskt")]
    [DisplayName("E-post")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Förnamn är obligatoriskt")]
    [DisplayName("Förnamn")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Efternamn är obligatoriskt")]
    [DisplayName("Efternamn")]
    public string LastName { get; set; } = "";

    [DisplayName("Fullständigt namn")]
    public string CompleteName { get { return FirstName + " " + LastName; } }

    [Required(ErrorMessage = "Personnummer är obligatoriskt")]
    [DisplayName("Personnummer")]
    public string SocialSecurityNumber { get; set; } = "";

    [Required(ErrorMessage = "Gatuadress är obligatoriskt")]
    [DisplayName("Gatuadress")]
    public string StreetAddress { get; set; } = "";

    [Required(ErrorMessage = "Postkod är obligatoriskt")]
    [DisplayName("Postkod")]
    public string PostalCode { get; set; } = "";

    [Required(ErrorMessage = "Telefonnummer är obligatoriskt")]
    [DisplayName("Telefonnummer")]
    public string Phone { get; set; } = "";

    [DisplayName("Är användaren en lärare?")]
    public bool IsATeacher { get; set; } = false;

    [Required(ErrorMessage = "Ett lösenord är obligatoriskt")]
    [DisplayName("Temporärt lösenord")]
    public string Password { get; set; } = "";
}
