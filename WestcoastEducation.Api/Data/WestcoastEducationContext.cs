using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Models;

namespace WestcoastEducation.Api.Data;
public class WestcoastEducationContext : DbContext
{
    public DbSet<CourseModel> Courses { get; set; }
    public DbSet<StudentModel> Students { get; set; }
    public DbSet<TeacherModel> Teachers { get; set; }
    public DbSet<TeacherSkillsModel> TeacherSkills { get; set; }

    public WestcoastEducationContext(DbContextOptions options) : base(options)
    {
    }
}
