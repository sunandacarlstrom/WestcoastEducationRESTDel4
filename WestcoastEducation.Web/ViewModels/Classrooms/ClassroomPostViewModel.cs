using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WestcoastEducation.Web.ViewModels.Classrooms;

public class ClassroomPostViewModel
{
    [Required(ErrorMessage = "Kursnummer är obligatoriskt")]
    [DisplayName("Kursnummer")]
    public string Number { get; set; } = "";

    [Required(ErrorMessage = "Kursnamn är obligatoriskt")]
    [DisplayName("Kursnamn")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Lärare är obligatoriskt")]
    [DisplayName("Lärare")]
    public string Teacher { get; set; }

    [Required(ErrorMessage = "Kurstitel är obligatoriskt")]
    [DisplayName("Kurstitel")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Kursinnehåll är obligatoriskt")]
    [DisplayName("Kursinnehåll (separera med '|')")]
    public string Content { get; set; } = "";

    [Required(ErrorMessage = "Startdatum är obligatoriskt")]
    [DisplayName("Startdatum")]
    public DateTime Start { get; set; }

    [Required(ErrorMessage = "Slutdatum är obligatoriskt")]
    [DisplayName("Slutdatum")]
    public DateTime End { get; set; }

    [DisplayName("Är kursen på distans?")]
    public bool IsOnDistance { get; set; }

    public List<SelectListItem> Teachers { get; set; }
}
