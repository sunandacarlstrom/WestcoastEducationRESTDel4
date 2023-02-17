using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Users;
using static System.Net.Mime.MediaTypeNames;

namespace WestcoastEducation.Web.Controllers;

[Route("admin/user")]
public class UserAdminController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;
    public UserAdminController(IConfiguration config, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config.GetSection("apiSettings:baseUrl").Value;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IActionResult> Index()
    {
        // skapa en instans av http klienten 
        using var client = _httpClient.CreateClient();

        // hämta datat ifrån api'et 
        var responseStudents = await client.GetAsync($"{_baseUrl}/students/listall");
        // ...om inte responsen är lyckad så retuneras ett felmeddelande
        if (!responseStudents.IsSuccessStatusCode) return View("_Error", new ErrorModel
        {
            ErrorTitle = "Det gick fel! Kunde inte hämta studenterna från API:et",
            ErrorMessage = responseStudents.ToString()
        });

        var responseTeachers = await client.GetAsync($"{_baseUrl}/teachers/listall");
        if (!responseTeachers.IsSuccessStatusCode) return View("_Error", new ErrorModel
        {
            ErrorTitle = "Det gick fel! Kunde inte hämta lärarna från API:et",
            ErrorMessage = responseTeachers.ToString()
        });

        // Om allt går bra... 
        // läs ut body (content) från mitt respons-paket 
        var jsonStudents = await responseStudents.Content.ReadAsStringAsync();
        var jsonTeachers = await responseTeachers.Content.ReadAsStringAsync();

        // deserializera json till en lista av objekt
        var studentList = JsonSerializer.Deserialize<List<UserListViewModel>>(jsonStudents, _options);
        var teacherList = JsonSerializer.Deserialize<List<UserListViewModel>>(jsonTeachers, _options);

        // ändra alla Teachers till true enligt min UserListViewModel
        teacherList.ForEach(t => t.IsATeacher = true);

        // slå ihop studentList och teacherList
        var userList = new List<UserListViewModel>();
        userList.AddRange(studentList);
        userList.AddRange(teacherList);

        return View("Index", userList);
    }

    [HttpGet("TeacherDetails/{userId}")]
    public async Task<IActionResult> TeacherDetails(int userId)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/teachers/getbyid/{userId}");

        if (!response.IsSuccessStatusCode) return Content("Åh nej det gick fel");

        var json = await response.Content.ReadAsStringAsync();

        var teacher = JsonSerializer.Deserialize<TeacherDetailsViewModel>(json, _options);

        return View("TeacherDetails", teacher);
    }

    [HttpGet("StudentDetails/{userId}")]
    public async Task<IActionResult> StudentDetails(int userId)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/students/getbyid/{userId}");

        if (!response.IsSuccessStatusCode) return Content("Åh nej det gick fel");

        var json = await response.Content.ReadAsStringAsync();

        var student = JsonSerializer.Deserialize<StudentDetailsViewModel>(json, _options);

        return View("StudentDetails", student);
    }

    [HttpGet("CreateTeacher")]
    public async Task<IActionResult> CreateTeacher()
    {
        // en lista av typen TeacherSkills
        var skillsList = new List<SelectListItem>();
        var coursesList = new List<SelectListItem>();

        // hämta datat ifrån api'et 
        using var client = _httpClient.CreateClient();

        var responseSkills = await client.GetAsync($"{_baseUrl}/teacherskills/listall");
        if (!responseSkills.IsSuccessStatusCode) return Content("Hoppsan det gick inget vidare!!!");

        var jsonSkills = await responseSkills.Content.ReadAsStringAsync();
        var skills = JsonSerializer.Deserialize<List<SkillsSettings>>(jsonSkills, _options);

        foreach (var skill in skills)
        {
            skillsList.Add(new SelectListItem { Value = skill.Id.ToString(), Text = skill.Skill });
        }

        var responseCourses = await client.GetAsync($"{_baseUrl}/courses/listall");
        if (!responseCourses.IsSuccessStatusCode) return Content("Hoppsan det gick inget vidare!!!");

        var jsonCourses = await responseCourses.Content.ReadAsStringAsync();
        var courses = JsonSerializer.Deserialize<List<CourseSettings>>(jsonCourses, _options);

        foreach (var course in courses)
        {
            coursesList.Add(new SelectListItem { Value = course.Id.ToString(), Text = course.Name });
        }

        // skapar en ny vymodell för att användaren ska kunna fylla i formuläret
        var teacher = new TeacherPostViewModel();
        teacher.Skills = skillsList;
        teacher.Courses = coursesList;

        return View("CreateTeacher", teacher);
    }

    [HttpPost("CreateTeacher")]
    public async Task<IActionResult> Create(TeacherPostViewModel teacher)
    {
        // kontrollerar att allt är korrekt utifrån det som har matats in av användaren efter att ha tryckt på knappen 'Spara'
        if (!ModelState.IsValid) return View("Create", teacher);

        // Om allt går bra... 
        // skapas ett nytt objekt, här är det som ska till api'et (just nu manuellt, men man kan också skicka en ny vymodell)
        var model = new
        {
            Name = teacher.Name,
            Email = teacher.Email,
            TeacherSkillIds = teacher.SkillsList,
            CourseIds = teacher.CoursesList
        };

        // skapar en ny klient 
        using var client = _httpClient.CreateClient();
        // istället för att läsa in data så skickas datat till api'et genom att skapa innehållet i form av ett JSON-paket 
        var body = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);
        // skickar över JSON-paketet till rätt endpoint i api'et 
        var response = await client.PostAsync($"{_baseUrl}/teachers", body);
        // kontrollerar att allting går bra... 
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        return Content("Det gick fel! Bättre lycka nästa gång");
    }
}