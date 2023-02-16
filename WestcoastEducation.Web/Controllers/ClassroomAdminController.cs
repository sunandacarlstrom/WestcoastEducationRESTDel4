using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Classrooms;
using static System.Net.Mime.MediaTypeNames;

namespace WestcoastEducation.Web.Controllers;

[Route("admin/classroom")]
public class ClassroomAdminController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;
    public ClassroomAdminController(IConfiguration config, IHttpClientFactory httpClient)
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
        var response = await client.GetAsync($"{_baseUrl}/courses/listall");
        //TODO: skicka istälelt en Error-sida om tid finns... 
        // kontrollerar om inte responsen är lyckad så retuneras ett felmeddelande
        if (!response.IsSuccessStatusCode) return Content("Åh nej det gick fel");

        // Om allt går bra... 
        // läs ut body (content) från mitt respons-paket 
        var json = await response.Content.ReadAsStringAsync();

        // deserializera json till en lista av objekt
        var classroom = JsonSerializer.Deserialize<IList<ClassroomListViewModel>>(json, _options);

        return View("Index", classroom);
    }

    [HttpGet("details/{classroomId}")]
    public async Task<IActionResult> Details(int classroomId)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/courses/getbyid/{classroomId}");

        if (!response.IsSuccessStatusCode) return Content("Åh nej det gick fel");

        var json = await response.Content.ReadAsStringAsync();

        var classroom = JsonSerializer.Deserialize<ClassroomDetailsViewModel>(json, _options);

        return View("Details", classroom);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        // en lista av typen Teachers 
        var teachersList = new List<SelectListItem>();

        // hämta datat ifrån api'et 
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/teachers/listall");
        if (!response.IsSuccessStatusCode) return Content("Hoppsan det gick inget vidare!!!");
        var json = await response.Content.ReadAsStringAsync();
        var teachers = JsonSerializer.Deserialize<List<CourseSettings>>(json, _options);

        foreach (var teacher in teachers)
        {
            teachersList.Add(new SelectListItem { Value = teacher.Name, Text = teacher.Name });
        }

        // skapar en ny vymodell för att användaren ska kunna fylla i formuläret
        var classroom = new ClassroomPostViewModel();
        classroom.Teachers = teachersList;

        return View("Create", classroom);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ClassroomPostViewModel classroom)
    {
        // kontrollerar att allt är korrekt utifrån det som har matats in av användaren efter att ha tryckt på knappen 'Spara'
        if (!ModelState.IsValid) return View("Create", classroom);

        // Om allt går bra... 
        // skapas ett nytt objekt, här är det som ska till api'et (just nu manuellt, men man kan också skicka en ny vymodell)
        var model = new
        {
            Number = classroom.Number,
            Name = classroom.Name,
            Teacher = classroom.Teacher,
            Title = classroom.Title,
            Content = "Test",
            Start = classroom.Start,
            End = classroom.End,
            IsOnDistance = classroom.IsOnDistance
        };

        // skapar en ny klient 
        using var client = _httpClient.CreateClient();
        // istället för att läsa in data så skickas datat till api'et genom att skapa innehållet i form av ett JSON-paket 
        var body = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, Application.Json);
        // skickar över JSON-paketet till rätt endpoint i api'et 
        var response = await client.PostAsync($"{_baseUrl}/courses", body);
        // kontrollerar att allting går bra... 
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        return Content("Det gick fel! Bättre lycka nästa gång");
    }
}