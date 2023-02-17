using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;
using WestcoastEducation.Api.ViewModels.Students;

namespace WestcoastEducation.Api.Controllers;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private readonly WestcoastEducationContext _context;
    public CoursesController(WestcoastEducationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Listar alla kurser i systemet
    /// </summary> 
    [HttpGet("listall")]
    //endast tillgänglig för användare med rollen Teacher
    // [Authorize(Roles = "Teacher, Admin")]
    public async Task<ActionResult> ListAll()
    {
        var result = await _context.Courses
        // talar om för EF Core att när du listar Courses vill jag också att du inkluderar det som finns i Teacher-tabellen där jag har en INNER JOIN maskning, 
        // alltså teacherId med ett visst värde i Courses måste existera i Teacher som id-kolumn.
        .Include(t => t.Teacher)
        // projicerar resultatet utifrån rätt ViewModel 
        // måste därför ha en vymodell (DTO) som jag kan flytta över data ifrån den här frågan till en modell som json kan retunera 
        .Select(c => new CourseListViewModel
        {
            // Här definierar jag vilka kolumner som jag egentligen vill ha tillbaka i (SQL)frågan 
            Id = c.Id,
            Teacher = c.Teacher.Name ?? "",
            Number = c.Number,
            Name = c.Name,
            Title = c.Title,
            Start = c.Start,
            End = c.End
        })
        // listar alla kurser
        .ToListAsync();
        return Ok(result);
    }

    /// <summary>
    /// Hämtar en kurs baserat på kurs-ID 
    /// </summary> 
    /// <param name="id">Kurs-ID</param> 
    /// <returns>
    /// Kursinformation om sökt kurs och dess lärare samt studenter
    /// </returns> 
    /// <response code="200">Retunerar kursinformation om sökt kurs och dess lärare samt studenter</response> 
    [HttpGet("getbyid/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _context.Courses
        .Include(t => t.Teacher)
        .Include(s => s.Students)
        .Select(c => new CourseDetailsViewModel
        {
            Id = c.Id,
            Teacher = c.Teacher.Name ?? "",
            // listar en lista av studenter i min lista av kurser
            Students = c.Students.Select(s => new StudentListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList(),
            Number = c.Number,
            Name = c.Name,
            Title = c.Title,
            Start = c.Start,
            End = c.End,
            Status = c.Status.ToString(),
            Content = c.Content ?? "",
        })

        // jag vill ha tag i ett Id som stämmer överrens med det Id som jag skickar in 
        .SingleOrDefaultAsync(c => c.Id == id);
        return Ok(result);
    }

    [HttpGet("getbycourseno/{courseNo}")]
    public async Task<ActionResult> GetByCourseNumber(string courseNo)
    {
        var result = await _context.Courses
        .Include(t => t.Teacher)
        .Include(s => s.Students)
        .Select(c => new CourseDetailsViewModel
        {
            Id = c.Id,
            Teacher = c.Teacher.Name ?? "",
            Students = c.Students.Select(s => new StudentListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList(),
            Number = c.Number,
            Name = c.Name,
            Title = c.Title,
            Start = c.Start,
            End = c.End,
            Status = c.Status.ToString(),
            Content = c.Content ?? ""
        })
        .SingleOrDefaultAsync(c => c.Number.ToUpper().Trim() == courseNo.ToUpper().Trim());
        return Ok(result);
    }

    [HttpGet("getbycoursetitle/{courseTitle}")]
    public async Task<ActionResult> GetByCourseTitle(string courseTitle)
    {
        var result = await _context.Courses
        .Include(t => t.Teacher)
        .Include(s => s.Students)
        // sätter ett villkor som beskriver vad jag vill söka på 
        .Where(s => s.Title.ToUpper().Trim() == courseTitle.ToUpper().Trim())
        .Select(c => new CourseDetailsViewModel
        {
            Id = c.Id,
            Teacher = c.Teacher.Name ?? "",
            Students = c.Students.Select(s => new StudentListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList(),
            Number = c.Number,
            Name = c.Name,
            Title = c.Title,
            Start = c.Start,
            End = c.End,
            Status = c.Status.ToString(),
            Content = c.Content ?? ""
        })
        // listar alla kurser som WU22 programmets studenter har under sin studietid
        .ToListAsync();
        return Ok(result);
    }

    [HttpGet("getbyteacher/{teacher}")]
    public async Task<ActionResult> GetByTeacher(string teacher)
    {
        var result = await _context.Courses
        .Include(t => t.Teacher)
        .Include(s => s.Students)
        .Where(s => s.Teacher.Name.ToUpper().Trim() == teacher.ToUpper().Trim())
        .Select(c => new CourseDetailsViewModel
        {
            Id = c.Id,
            Teacher = c.Teacher.Name ?? "",
            Students = c.Students.Select(s => new StudentListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList(),
            Number = c.Number,
            Name = c.Name,
            Title = c.Title,
            Start = c.Start,
            End = c.End,
            Status = c.Status.ToString(),
            Content = c.Content ?? ""
        })
        // listar alla kurser som en specifik lärare håller i 
        .ToListAsync();
        return Ok(result);
    }

    [HttpGet("getbycoursestart/{year}/{month}/{day}")]
    public async Task<ActionResult> GetByCourseStart(int year, int month, int day)
    {
        // skapar en DateTime-variabel för att kunna använda ".CompareTo" nedanför
        DateTime search = new DateTime(year, month, day);

        var result = await _context.Courses
        .Include(t => t.Teacher)
        .Include(s => s.Students)
        .Where(s => s.Start.CompareTo(search) == 0)
        .Select(c => new CourseDetailsViewModel
        {
            Id = c.Id,
            Teacher = c.Teacher.Name ?? "",
            Students = c.Students.Select(s => new StudentListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList(),
            Number = c.Number,
            Name = c.Name,
            Title = c.Title,
            Start = c.Start,
            End = c.End,
            Status = c.Status.ToString(),
            Content = c.Content ?? ""
        })
        // listar alla kurser som startar samtidigt
        .ToListAsync();
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult> AddCourse(CourseAddViewModel model)
    {
        // kontrollerar att jag har fått in allting korrekt... 
        // BadRequest ger statuskoden 400
        if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna lägga till kursen i systemet");

        // kontrollerar att kursen inte redan finns i systemet...
        var exists = await _context.Courses.SingleOrDefaultAsync(c => c.Number.ToUpper().Trim() == model.Number.ToUpper().Trim());

        // om exits inte är null då skickas en BadRequest...
        if (exists is not null) return BadRequest($"Vi har redan registrerat en kurs med kursnummer {model.Number}");

        // kontrollera att läraren finns i systemet... 
        var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Name.ToUpper().Trim() == model.Teacher.ToUpper().Trim());

        // om läraren inte finns ...
        if (teacher is null) return NotFound($"Vi kunde inte hitta någon lärare med namnet {model.Teacher} i vårt system");

        // skapar CourseModel som ska skickas till databasen 
        var course = new CourseModel
        {
            Number = model.Number,
            Teacher = teacher,
            Name = model.Name,
            Title = model.Title,
            Status = model.Status,
            Content = model.Content
        };

        await _context.Courses.AddAsync(course);

        if (await _context.SaveChangesAsync() > 0)
        {
            var added = new CourseDetailsViewModel
            {
                Id = course.Id,
                Teacher = course.Teacher.Name ?? "",
                Number = course.Number,
                Name = course.Name,
                Title = course.Title,
                Start = course.Start,
                End = course.End,
                Status = course.Status.ToString(),
                Content = course.Content ?? ""
            };
            //retunerar också ett nytt objekt 
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, added);
        }

        // när jag gör SaveChangesAsync i ett if-uttryck så måste jag här retunera ett felmeddelande som säger "Ett oväntat fel har uppstått"
        return StatusCode(500, "Internal Server Error");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCourse(int id, CourseUpdateViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna uppdatera kursen");

        // genom att söka på id med ".FindAsync" kan vi snabbare kontrollerar att kursen inte redan finns i systemet
        var course = await _context.Courses.FindAsync(id);

        // om course är null då skickas en BadRequest
        if (course is null) return BadRequest($"Vi kan inte hitta en kurs i systemet med {model.Number}");

        // kontrollera att läraren finns i systemet
        var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Name.ToUpper().Trim() == model.Teacher.ToUpper().Trim());

        // om läraren inte finns...
        if (teacher is null) return NotFound($"Vi kunde inte hitta någon lärare med namnet {model.Teacher} i vårt system");

        // flytta över all information i vår modell till UpdateViewModel
        // Put ersätter/uppdaterar alla egenskaper med ny information 
        course.Teacher = teacher;
        course.Number = model.Number;
        course.Name = model.Name;
        course.Title = model.Title;
        course.Start = model.Start;
        course.End = model.End;
        course.Status = model.Status;
        course.Content = model.Content;

        // tala om för context och Courses att jag har som syfte att göra en uppdatering
        _context.Courses.Update(course);

        //kontrollera att jag har får tillbaka något som har ändrats 
        if (await _context.SaveChangesAsync() > 0)
        {
            // Gå till databasen och uppdatera en ny kurs...
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("addstudent/{courseId}")]
    public async Task<ActionResult> AddStudent(int courseId, CourseAddStudentViewModel model)
    {
        var course = await _context.Courses
        .SingleOrDefaultAsync(t => t.Id == courseId);

        if (course is null) return BadRequest($"Vi kunde inte hitta en kurs med id {courseId}");

        var student = await _context.Students.FindAsync(model.Id);
        if (student is null) return NotFound($"Tyvärr kunde vi inte hitta någon student med id {model.Id}");

        //Om inte är initierad skapas en lista för det inte ska bli null när man lägger till kompetens nedanför
        //eftersom en kurs kan ha flera studenter
        if (course.Students is null) course.Students = new List<StudentModel>();

        course.Students.Add(student);

        _context.Update(course);

        //kontrollera att jag har får tillbaka något som har ändrats 
        if (await _context.SaveChangesAsync() > 0)
        {
            // Gå till databasen och uppdatera en lärare...
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("setstatus/asfull/{id}")]
    public async Task<ActionResult> SetStatusAsFull(int id)
    {
        // hitta kursen  
        var course = await _context.Courses.FindAsync(id);

        // kontrollerar att kursen existerar
        if (course is null) return NotFound($"Vi kan inte hitta någon kurs med id: {id}");

        // om den finns så kan vi sätta/uppdatera kursens status
        course.Status = CourseStatusEnum.FullyBooked;

        _context.Courses.Update(course);

        if (await _context.SaveChangesAsync() > 0)
        {
            // Gå till databasen och markera en kurs som fullbokad...
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("setstatus/asdone/{id}")]
    public async Task<ActionResult> SetStatusAsDone(int id)
    {
        // hitta kursen  
        var course = await _context.Courses.FindAsync(id);

        // kontrollerar att kursen existerar
        if (course is null) return NotFound($"Vi kan inte hitta någon kurs med id: {id}");

        // om den finns så kan vi sätta/uppdatera kursens status
        course.Status = CourseStatusEnum.Completed;

        _context.Courses.Update(course);

        if (await _context.SaveChangesAsync() > 0)
        {
            // Gå till databasen och markera en kurs som avklarad...
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("setteacher/{id}")]
    public async Task<ActionResult> SetTeacher(int id, CourseSetTeacherViewModel model)
    {
        // hitta kursen  
        var course = await _context.Courses.FindAsync(id);

        // kontrollerar att kursen existerar
        if (course is null) return NotFound($"Vi kan inte hitta någon kurs med id: {id}");

        // om den finns så kan vi sätta en lärare till kursen
        course.TeacherId = model.TeacherId;

        _context.Courses.Update(course);

        if (await _context.SaveChangesAsync() > 0)
        {
            // Gå till databasen och sätter en lärare till kursen 
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course is null) return NotFound($"Vi kan inte hitta någon kurs med id: {id}");

        _context.Courses.Remove(course);

        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }
}