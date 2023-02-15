using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestcoastEducation.Api.Models;
public class CourseModel
{
    [Key]
    public int Id { get; set; }
    public int? TeacherId { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public CourseStatusEnum Status { get; set; }

    // The One-Side (composition)
    [ForeignKey("TeacherId")]
    public TeacherModel Teacher { get; set; }

    //The Many-Side (aggregation)
    public ICollection<StudentModel> Students { get; set; }
}
