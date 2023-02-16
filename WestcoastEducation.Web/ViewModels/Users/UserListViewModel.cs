namespace WestcoastEducation.Web.ViewModels.Users;

public class UserListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsATeacher { get; set; } = false;
}
