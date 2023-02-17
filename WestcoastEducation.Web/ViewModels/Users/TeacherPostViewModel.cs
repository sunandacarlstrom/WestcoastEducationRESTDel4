using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WestcoastEducation.Web.ViewModels.Users;
public class TeacherPostViewModel
{
    [DisplayName("AnvändarID")]
    public int Id { get; set; }

    [DisplayName("Namn")]
    public string Name { get; set; }

    [DisplayName("E-postadress")]
    public string Email { get; set; }

    public List<SelectListItem> Skills { get; set; }

    [DisplayName("Kurser (Välj flera med CTRL/CMD+vänsterklick)")]

    public List<int> SkillsList { get; set; }

    public List<SelectListItem> Courses { get; set; }
    
    [DisplayName("Kurser (Välj flera med CTRL/CMD+vänsterklick)")]
    public List<int> CoursesList { get; set; }
}
