using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels;
public class TeacherUpdateViewModel
{
    [Required(ErrorMessage = "Lärarens namn saknas")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Lärarens e-postadress saknas")]
    public string? Email { get; set; }
}
