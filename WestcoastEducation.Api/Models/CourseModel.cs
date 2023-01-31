using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.Models
{
    public class CourseModel
    {
        [Key]
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Length { get => End - Start; }
        public CourseTypeEnum Type { get; set; }
    }
}