using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Models;

namespace WestcoastEducation.Api.Data;

public class WestcoastEducationContext : DbContext
{
    public DbSet<CourseModel> Courses => Set<CourseModel>();
    public DbSet<StudentModel> Students => Set<StudentModel>();
    public DbSet<TeacherModel> Teachers => Set<TeacherModel>();

    public WestcoastEducationContext(DbContextOptions options) : base(options)
    {
    }
}
