using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.Models;

public class StudentModel
{
    [Key]
    public int Id { get; set; }
    public string? Email { get; set; }

    public ICollection<CourseModel>? Courses { get; set; }
}
