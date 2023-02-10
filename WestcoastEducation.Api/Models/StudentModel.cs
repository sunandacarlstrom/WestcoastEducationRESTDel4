using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestcoastEducation.Api.Models;
public class StudentModel
{
    [Key]
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

    // The One-Side
    [ForeignKey("CourseId")]
    public CourseModel? Course { get; set; }
}
