using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestcoastEducation.Api.Models
{
    public class CourseModel
    {
        [Key]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public CourseTypeEnum Type { get; set; }

        // The One-Side
        [ForeignKey("TeacherId")]
        public TeacherModel Teacher { get; set; } = new TeacherModel();

        //The Many-Side
        public ICollection<StudentModel>? Students { get; set; }
    }
}