namespace WestcoastEducation.Api.ViewModels;
public class CourseDetailsViewModel
{
    public int Id { get; set; }
    public string Teacher { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Status { get; set; }
    public string Content { get; set; }

    public ICollection<StudentListViewModel> Students { get; set; }
}
