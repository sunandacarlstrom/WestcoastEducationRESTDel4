using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels;

public class CourseUpdateViewModel
{
    [Required(ErrorMessage = "Lärare för kursen saknas")]
    public string Teacher { get; set; } = "";

    [Required(ErrorMessage = "Kursnummer saknas")]
    [StringLength(12)]
    public string? Number { get; set; }

    [Required(ErrorMessage = "Kursnamn saknas")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Kurstitel saknas")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Startdatum saknas")]
    public DateTime Start { get; set; }

    [Required(ErrorMessage = "Slutdatum saknas")]
    public DateTime End { get; set; }

    public string Content { get; set; } = "";
}
