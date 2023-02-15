using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Data
{
    public class WestcoastEducationContext : DbContext
    {
        // Skapar kopplingen mellan min databas och mina klasser
        public DbSet<ClassroomModel> Classrooms => Set<ClassroomModel>();
        public DbSet<UserModel> Users => Set<UserModel>();

        public WestcoastEducationContext(DbContextOptions options) : base(options) { }
    }
}