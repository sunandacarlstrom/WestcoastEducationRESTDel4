namespace WestcoastEducation.Api.ViewModels.TeacherSkills;
public class TeacherSkillsDetailsViewModel
{
    public int Id { get; set; }
    public string? Skill { get; set; }
    public ICollection<TeacherListViewModel>? Teachers { get; set; }
}
