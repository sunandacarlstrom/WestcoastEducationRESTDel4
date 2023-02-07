using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Api.ViewModels;

public class StudentListViewModel
{
    public int Id { get; set; }
    public string Course { get; set; } = ""; 
    public string? Name { get; set; }
    public string? Email { get; set; }
}
