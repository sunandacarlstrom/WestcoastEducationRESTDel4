using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.ViewModels.Account;

namespace WestcoastEducation.Web.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;
    public AccountController(IConfiguration config, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config.GetSection("apiSettings:baseUrl").Value;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    [HttpGet("register")]
    // P.S. denna metod är synkront eftersom den ej pratar med api:et, utan levererar endast ett formulär
    public IActionResult Register()
    {
        // skapar en ny modell för att kunna registrera användaren
        var registerModel = new RegisterUserViewModel();
        // anropar vyn Register och skickar över modellen 
        return View("Register", registerModel);
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        return View("Login");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // skapa en instans av http klienten 
        using var client = _httpClient.CreateClient();

        if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna logga in");

        //hämta datat ifrån api'et 
        // var response = await client.PostAsync($"{_baseUrl}/account/login/{model}", new JsonContent());
        var response = await client.PostAsJsonAsync($"{_baseUrl}/account/login", model, CancellationToken.None);

        if (!response.IsSuccessStatusCode) return BadRequest("Felaktig inmatning");

        // extract token from response body
        string token = await response.Content.ReadAsStringAsync();

        // set token as cookie in response
        Response.Cookies.Append("AuthToken", token);

        return RedirectToAction("Index", "admin"); 
    }
}