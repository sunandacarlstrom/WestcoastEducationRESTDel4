using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.ViewModels.Users;

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
        //TODO: skicka istälelt en Error-sida om tid finns... 
        // kontrollerar om inte responsen är lyckad så retuneras ett felmeddelande
        if (!responseStudents.IsSuccessStatusCode) return Content("Åh nej något gick fel att lista studenter");

        var responseTeachers = await client.GetAsync($"{_baseUrl}/teachers/listall");
        if (!responseTeachers.IsSuccessStatusCode) return Content("Åh nej det gick fel att lista lärare");

        // Om allt går bra... 
        // läs ut body (content) från mitt respons-paket 
        var jsonStudents = await responseStudents.Content.ReadAsStringAsync();
        var jsonTeachers = await responseTeachers.Content.ReadAsStringAsync();

        // deserializera json till en lista av objekt
        var studentList = JsonSerializer.Deserialize<List<UserListViewModel>>(jsonStudents, _options);
        var teacherList = JsonSerializer.Deserialize<List<UserListViewModel>>(jsonTeachers, _options);

        // ändra alla Teachers till true enligt min UserListViewModel
        teacherList.ForEach(t => t.IsATeacher = true);

        //slå ihop studentList och teacherList
        var userList = new List<UserListViewModel>();
        userList.AddRange(studentList);
        userList.AddRange(teacherList);

        return View("Index", userList);
    }
}