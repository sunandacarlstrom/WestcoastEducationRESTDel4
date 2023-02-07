using WestcoastEducation.Api.Models;

namespace WestcoastEducation.Api.ViewModels;

public class TeacherAddViewModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }

    public IList<TeacherSkillsModel> TeacherSkills { get; set; }
}
