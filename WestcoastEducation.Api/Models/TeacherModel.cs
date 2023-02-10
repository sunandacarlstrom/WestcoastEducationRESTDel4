using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.Models;
public class TeacherModel
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

    //The Many-Side 
    public ICollection<CourseModel>? Courses { get; set; }

    //The Many-Side
    public ICollection<TeacherSkillsModel>? Skills { get; set; }
}
