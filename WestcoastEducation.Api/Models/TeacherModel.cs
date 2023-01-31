using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.Models;

public class TeacherModel
{
    [Key]
    public int Id { get; set; }
    public string? Email { get; set; }
}
