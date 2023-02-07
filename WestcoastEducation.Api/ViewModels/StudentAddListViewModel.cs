using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels;

public class StudentAddListViewModel
{
    // [Required(ErrorMessage = " för kursen saknas")] ????
    // ska jag ens ha med denna?? Tog bort studentId eftersom det genereras automatiskt
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Studentens namn saknas")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Studentens e-postadress saknas")]
    public string? Email { get; set; }
}
