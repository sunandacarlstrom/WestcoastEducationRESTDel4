using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Classrooms;

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

    [HttpPost("create")]
    public async Task<IActionResult> Create()
    {
        // en lista av typen Teachers 
        var teachersList = new List<SelectListItem>();

        // hämta datat ifrån api'et 
        using var client = _httpClient.CreateClient(); 
        var response = await client.GetAsync($"{_baseUrl}/teachers/listall"); 
        if(!response.IsSuccessStatusCode) return Content("Hoppsan det gick inget vidare!!!"); 
        var json = await response.Content.ReadAsStringAsync(); 
        var teachers = JsonSerializer.Deserialize<List<CourseSettings>>(json, _options); 

        // skapar en ny vymodell
        var classroom = new ClassroomPostViewModel();
        classroom.Teachers = teachersList; 

        return View("Create", classroom);
    }
}


#region old
/*
public class ClassroomAdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ClassroomAdminController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // går direkt till UnitOfWork och hittar rätt metod 
            var classrooms = await _unitOfWork.ClassroomRepository.ListAllAsync();

            // här görs en projicering med hjälp av LINQ, dvs. jag vill ta all data som finns i ClassroomModel och gör ett nytt objekt
            // för varje kurs i den listan kommer det ske en intern loop och skapar ett nytt ClassroomListViewModel
            var model = classrooms.Select(c => new ClassroomListViewModel
            {
                Id = c.Id,
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Content = c.Content,
                Start = c.Start,
                End = c.End,
                IsOnDistance = c.IsOnDistance
                // typar om min projicering till en IList eftersom min model per automatik vill ta emot en IEnumerable
            }).ToList();

            return View("Index", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett oväntat fel har inträffat vid inhämtning av kurser",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        // skapa ett nytt objekt för att skicka över till vyn
        var classroom = new ClassroomPostViewModel();
        return View("Create", classroom);
    }

    [HttpPost("create")]
    // Förändrat vad som förväntas att få in som argument 
    public async Task<IActionResult> Create(ClassroomPostViewModel classroom)
    {
        try
        {
            // skriver ut felmeddelandet direkt i vyn med hjälp av dekorations attributen i ClassroomPostViewModel
            if (!ModelState.IsValid) return View("Create", classroom);

            // söker efter ett kursnummer lika med det som kommer in i anropet via mitt UnitOfWork
            var exists = await _unitOfWork.ClassroomRepository.FindByNumberAsync(classroom.Number);

            // kontrollerar om detta nummer redan existerar
            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när kursen skulle sparas",
                    ErrorMessage = $"En kurs med numret {classroom.Number} finns redan i systemet"
                };

                // skicka tillbaka en vy som visar information gällande felet 
                return View("_Error", error);
            }

            // här görs en manuell konvertering till typen ClassroomModel som förväntas i vår datacontext 
            var classrooomToAdd = new ClassroomModel
            {
                Number = classroom.Number,
                Name = classroom.Name,
                Title = classroom.Title,
                Content = classroom.Content,
                Start = classroom.Start,
                End = classroom.End,
                IsOnDistance = classroom.IsOnDistance,
            };

            // lägg upp kursen i minnet via mitt UnitOfWork
            if (await _unitOfWork.ClassroomRepository.AddAsync(classrooomToAdd))
            {
                // spara ner i databas via mitt UnitOfWork
                if (await _unitOfWork.Complete())
                {
                    // Om allt går bra, inga fel inträffar...
                    return RedirectToAction(nameof(Index));
                }
            }

            var saveError = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle sparas",
                ErrorMessage = $"Det inträffade ett fel när kursen med kursnumret {classroom.Number} skulle sparas"
            };

            // skicka tillbaka en vy som visar information gällande felet 
            return View("_Error", saveError);
        }
        // Ett annat fel har inträffat som vi inte har räknat med...
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpGet("edit/{classroomId}")]
    public async Task<IActionResult> Edit(int classroomId)
    {
        try
        {
            // får tillbaka en kurs och skicka till en vy
            // här vill jag alltså få tag i en kurs med Id som är lika med det som kommer in i metodanropet via mitt UnitOfWork
            var result = await _unitOfWork.ClassroomRepository.FindByIdAsync(classroomId);

            // kontrollerar om jag inte hittar kursen så skickas ett felmeddelande ut 
            if (result is null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när en kurs skulle hämtas för redigering",
                    ErrorMessage = $"Hittar ingen kurs med id {classroomId}"
                };

                return View("_Error", error);
            }

            // Om jag hittar kursen då retuneras vyn ClassroomUpdateViewModel
            var model = new ClassroomUpdateViewModel
            {
                Id = result.Id,
                Number = result.Number,
                Name = result.Name,
                Title = result.Title,
                Content = result.Content,
                Start = result.Start,
                End = result.End,
                IsOnDistance = result.IsOnDistance,
            };

            return View("Edit", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när en kurs skulle hämtas för redigering",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpPost("edit/{classroomId}")]
    public async Task<IActionResult> Edit(int classroomId, ClassroomUpdateViewModel classroom)
    {
        try
        {
            //skriver ut felmeddelandet direkt i vyn med hjälp av dekorations attributen i ClassroomUpdateViewModel
            if (!ModelState.IsValid) return View("Edit", classroom);

            // vara säker på att kursen jag vill redigera/uppdatera verkligen finns i Changetracking listan
            var classroomToUpdate = await _unitOfWork.ClassroomRepository.FindByIdAsync(classroomId);

            if (classroomToUpdate is null) return RedirectToAction(nameof(Index));

            classroomToUpdate.Number = classroom.Number;
            classroomToUpdate.Name = classroom.Name;
            classroomToUpdate.Title = classroom.Title;
            classroomToUpdate.Start = classroom.Start;
            classroomToUpdate.End = classroom.End;

            //uppdatera en kurs via ef 
            if (await _unitOfWork.ClassroomRepository.UpdateAsync(classroomToUpdate))
            {
                // Om allt går bra...
                // sparas det ner i databas (alla ändringar på en o samma gång)
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när redigering av kursen skulle sparas",
                ErrorMessage = $"Ett fel inträffade när vi skulle uppdatera kursen med kursnumret {classroomToUpdate.Number}"
            };

            return View("_Error", error);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när redigering av kursen skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [Route("delete/{classroomId}")]
    public async Task<IActionResult> Delete(int classroomId)
    {
        try
        {
            //hämta in kursen som jag vill radera via mitt UnitOfWork
            var classroomToDelete = await _unitOfWork.ClassroomRepository.FindByIdAsync(classroomId);

            if (classroomToDelete is null) return RedirectToAction(nameof(Index));

            // radera kursen
            if (await _unitOfWork.ClassroomRepository.DeleteAsync(classroomToDelete))
            {
                // spara ner i databas (alla ändringar på en o samma gång)
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle raderas",
                ErrorMessage = $"Ett fel inträffade när kursen med kursnumret {classroomToDelete.Number} skulle tas bort"
            };

            return View("_Error", error);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle raderas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }
}
*/
#endregion