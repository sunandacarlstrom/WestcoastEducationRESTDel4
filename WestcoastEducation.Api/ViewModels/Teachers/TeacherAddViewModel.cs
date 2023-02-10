using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels;
public class TeacherAddViewModel
{
    [Required(ErrorMessage = "Lärarens namn saknas")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Lärarens e-postadress saknas")]
    public string? Email { get; set; }

    public ICollection<int>? CourseIds { get; set; }
    public ICollection<int>? TeacherSkillIds { get; set; }
}
