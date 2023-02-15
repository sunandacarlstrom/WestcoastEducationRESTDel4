using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels.TeacherSkills;

public class TeacherSkillsAddViewModel
{
    [Required(ErrorMessage = "Lärarens kompetens saknas")]
    public string Skill { get; set; }

    public int? TeacherId { get; set; }
}
