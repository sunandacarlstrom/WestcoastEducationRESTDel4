using System.ComponentModel;

namespace WestcoastEducation.Web.ViewModels.Users;

public class StudentDetailsViewModel
{
    [DisplayName("Student-ID")]
    public int Id { get; set; }

    [DisplayName("Namn")]
    public string Name { get; set; }

    [DisplayName("E-postadress")]
    public string Email { get; set; }

    [DisplayName("Antagen på följande kurs")]
    public string Course { get; set; }
}
