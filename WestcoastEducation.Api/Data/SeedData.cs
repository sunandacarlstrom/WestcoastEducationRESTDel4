using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using WestcoastEducation.Api.Models;

namespace WestcoastEducation.Api.Data;
public static class SeedData
{

    public static async Task LoadRolesAndUsers(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
    {
        // om det inte finns någoting i min roleManager då vill jag kunna skapa 3 olika roller
        if (!roleManager.Roles.Any())
        {
            var admin = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
            var student = new IdentityRole { Name = "Student", NormalizedName = "STUDENT" };
            var teacher = new IdentityRole { Name = "Teacher", NormalizedName = "TEACHER" };

            // skapa och spara direkt till databasen 
            await roleManager.CreateAsync(admin);
            await roleManager.CreateAsync(student);
            await roleManager.CreateAsync(teacher);
        }

        // om det inte finns någoting i min userManager då vill jag kunna skapa användare
        if (!userManager.Users.Any())
        {
            var admin = new UserModel
            {
                UserName = "sunanda.carlstrom@gmail.com",
                Email = "sunanda.carlstrom@gmail.com",
                FirstName = "Sunanda",
                LastName = "Carlström"
            };

            // skapa och spara direkt till databasen 
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            // placera användaren Sunanda i rollen admin och gör så att hon kommer åt alla roller i systemet  
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Student", "Teacher" });

            // skapar en ny användare 
            var student = new UserModel
            {
                UserName = "carola@gmail.com",
                Email = "carola@gmail.com",
                FirstName = "Carola",
                LastName = "Assaf"
            };

            await userManager.CreateAsync(student, "Pa$$w0rd");
            await userManager.AddToRoleAsync(student, "Student");

            // skapar ytterligare en användare 
            var teacher = new UserModel
            {
                UserName = "adam@gmail.com",
                Email = "adam@gmail.com",
                FirstName = "Adam",
                LastName = "Fritz"
            };

            await userManager.CreateAsync(teacher, "Pa$$w0rd");
            await userManager.AddToRoleAsync(teacher, "Teacher");
        }
    }

    public static async Task LoadCourseData(WestcoastEducationContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.Courses.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/courses.json");
        var courses = JsonSerializer.Deserialize<List<CourseModel>>(json, options);

        if (courses is not null && courses.Count > 0)
        {
            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadStudentData(WestcoastEducationContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.Students.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/students.json");
        var students = JsonSerializer.Deserialize<List<StudentModel>>(json, options);

        if (students is not null && students.Count > 0)
        {
            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadTeacherData(WestcoastEducationContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.Teachers.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/teachers.json");
        var teachers = JsonSerializer.Deserialize<List<TeacherModel>>(json, options);

        if (teachers is not null && teachers.Count > 0)
        {
            await context.Teachers.AddRangeAsync(teachers);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadTeacherSkillsData(WestcoastEducationContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.TeacherSkills.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/teacherSkills.json");
        var skills = JsonSerializer.Deserialize<List<TeacherSkillsModel>>(json, options);

        if (skills is not null && skills.Count > 0)
        {
            await context.TeacherSkills.AddRangeAsync(skills);
            await context.SaveChangesAsync();
        }
    }
}
