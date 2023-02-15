using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels;
public class StudentAddListViewModel
{
    public string Course { get; set; }

    [Required(ErrorMessage = "Studentens namn saknas")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Studentens e-postadress saknas")]
    public string Email { get; set; }
}
