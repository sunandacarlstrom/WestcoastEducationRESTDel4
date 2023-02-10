using WestcoastEducation.Api.Models;

namespace WestcoastEducation.Api.ViewModels;
public class TeacherDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    //The Many-Side 
    public ICollection<CourseListViewModel> Courses { get; set; }

    //The Many-Side
    public ICollection<TeacherSkillsListViewModel> Skills { get; set; }
}