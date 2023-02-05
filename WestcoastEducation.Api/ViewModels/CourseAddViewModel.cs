using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels;

public class CourseAddViewModel
{
    [Required(ErrorMessage = "Lärare för kursen saknas")]
    public string Teacher { get; set; } = "";

    [Required(ErrorMessage = "Kursnummer saknas")]
    [StringLength(11)]
    public string? Number { get; set; }

    [Required(ErrorMessage = "Kursnamn saknas")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Kurstitel saknas")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Startdatum saknas")]
    public DateTime Start { get; set; }

    [Required(ErrorMessage = "Slutdatum saknas")]
    public DateTime End { get; set; }

    // sätter ej Required här för att tala om för klienten att det inte gör något om detta fält inte fylls i men att det är bra om det gör det.
    public string Content { get; set; } = "";
}
