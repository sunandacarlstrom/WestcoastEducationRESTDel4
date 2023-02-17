using System.ComponentModel;
using WestcoastEducation.Web.ViewModels.Classrooms;
using WestcoastEducation.Web.ViewModels.TeacherSkills;

namespace WestcoastEducation.Web.ViewModels.Users;

public class TeacherDetailsViewModel
{
    [DisplayName("AnvändarID")]
    public int Id { get; set; }

    [DisplayName("Namn")]
    public string Name { get; set; }

    [DisplayName("E-postadress")]
    public string Email { get; set; }

    [DisplayName("Kompetensområde")]
    public List<TeacherSkillsListViewModel> Skills { get; set; }

    [DisplayName("Undervisar i följande kurser")]
    public List<ClassroomListViewModel> Courses { get; set; }
}
