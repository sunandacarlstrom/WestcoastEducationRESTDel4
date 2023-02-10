namespace WestcoastEducation.Api.ViewModels;
public class StudentDetailsViewModel
{
    public int Id { get; set; }
    public string Course { get; set; } = "";
    public string? Name { get; set; }
    public string? Email { get; set; }
}
