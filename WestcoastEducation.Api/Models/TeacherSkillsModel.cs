using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestcoastEducation.Api.Models;

public class TeacherSkillsModel
{
    [Key]
    public int Id { get; set; }
    public string? Skill { get; set; }
}
