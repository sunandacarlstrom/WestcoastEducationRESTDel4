using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Classrooms;

namespace WestcoastEducation.Web.Controllers;

[Route("classroom")]
public class ClassroomController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;
    public ClassroomController(IConfiguration config, IHttpClientFactory httpClient)
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

        //hämta datat ifrån api'et 
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

        var classroom = JsonSerializer.Deserialize<ClassroomPublicDetailsViewModel>(json, _options);

        return View("Details", classroom);
    }
}
