using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.ViewModels.Account.Admin;

public class RoleViewModel
{
    [Required(ErrorMessage = "Namn på rollen saknas")]
    [DisplayName("Roll")]
    public string RoleName { get; set; }
}
