using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public CoursesController(WestcoastEducationContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult> List()
        {
            var result = await _context.Courses
            // talar om för EF Core att när du listar Courses vill jag också att du inkluderar det som finns i Teacher-tabellen där jag har en INNER JOIN masking, 
            // alltså teacherId med ett visst värde i Courses måste existera i Teacher som id-kolumn.
            .Include(t => t.Teacher)
            // projicerar resultatet in i min ViewListModel 
            .Select(c => new CourseListViewModel
            {
                //Här definierar jag vilka kolumner som jag egentligen vill ha tillbaka i (SQL)frågan 
                Id = c.Id,
                // använder en coalesce operation
                Teacher = c.Teacher.Name ?? "",
                Number = c.Number,
                Name = c.Name,
                Title = c.Title
            })
            .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _context.Courses
            .Include(t => t.Teacher)
            // måste ha en vymodell (DTO) som vi kan flytta över data ifrån den här frågan till en modell som json kan retunera 
            // projicerar resultatet 
            .Select(c => new CourseDetailsViewModel
            {
                //Här definierar jag vilka kolumner som jag egentligen vill ha tillbaka i (SQL)frågan 
                Id = c.Id,
                // använder en coalesce operation
                Teacher = c.Teacher.Name ?? "",
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Start = c.Start,
                End = c.End,
                Content = c.Content
            })
            // jag vill ha tag i ett Id som stämmer överrens med det Id som jag skickar in 
            .SingleOrDefaultAsync(c => c.Id == id);
            return Ok(result);
        }

        [HttpGet("courseno/{courseNo}")]
        public async Task<ActionResult> GetByCourseNumber(string courseNo)
        {
            var result = await _context.Courses
            .Include(t => t.Teacher)
            .Select(c => new CourseDetailsViewModel
            {
                Id = c.Id,
                Teacher = c.Teacher.Name ?? "",
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Start = c.Start,
                End = c.End,
                Content = c.Content
            })
            .SingleOrDefaultAsync(c => c.Number!.ToUpper().Trim() == courseNo.ToUpper().Trim());
            return Ok(result);
        }

        [HttpGet("coursetitle/{courseTitle}")]
        public async Task<ActionResult> GetByCourseTitle(string courseTitle)
        {
            var result = await _context.Courses
            .Include(t => t.Teacher)
            .Where(s => s.Title!.ToUpper().Trim() == courseTitle.ToUpper().Trim())
            .Select(c => new CourseDetailsViewModel
            {
                Id = c.Id,
                Teacher = c.Teacher.Name ?? "",
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Start = c.Start,
                End = c.End,
                Content = c.Content
            })
            // listar alla kurser som WU22 programmets elever har under sin studietid
            .ToListAsync();
            return Ok(result);
        }

        [HttpGet("teacher/{teacher}")]
        public async Task<ActionResult> GetByTeacher(string teacher)
        {
            var result = await _context.Courses
            .Include(t => t.Teacher)
            // sätter ett villkor där jag beskriver läraren som håller i x kurser
            .Where(s => s.Teacher.Name!.ToUpper().Trim() == teacher.ToUpper().Trim())
            .Select(c => new CourseDetailsViewModel
            {
                Id = c.Id,
                Teacher = c.Teacher.Name ?? "",
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Start = c.Start,
                End = c.End,
                Content = c.Content
            })
            // listar alla kurser som en specifik lärare håller i 
            .ToListAsync();
            return Ok(result);
        }

        [HttpGet("coursestart/{year}/{month}/{day}")]
        public async Task<ActionResult> GetByCourseStart(int year, int month, int day)
        {
            // skapar en DateTime-variabel för att kunna använda ".CompareTo" nedanför
            DateTime search = new DateTime(year, month, day);

            var result = await _context.Courses
            .Include(t => t.Teacher)
            .Where(s => s.Start.CompareTo(search) == 0)
            .Select(c => new CourseDetailsViewModel
            {
                Id = c.Id,
                Teacher = c.Teacher.Name ?? "",
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Start = c.Start,
                End = c.End,
                Content = c.Content
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
            if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna lagra kursen i systemet");

            // kontrollerar att kursen inte redan finns i systemet...
            var exists = await _context.Courses.SingleOrDefaultAsync(c => c.Number!.ToUpper().Trim() == model.Number!.ToUpper().Trim());

            // om exits inte är null då skickas en BadRequest...
            if (exists is not null) return BadRequest($"Vi har redan registrerat en kurs med kursnummer {model.Number}");

            // kontrollera att läraren finns i systemet... 
            var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Teacher.ToUpper().Trim());

            // om läraren inte finns ...
            if (teacher is null) return NotFound($"Vi kunde inte hitta någon lärare med namnet {model.Teacher} i vårt system");

            // skapar CourseModel som ska skickas till databasen 
            var course = new CourseModel
            {
                Number = model.Number,
                Teacher = teacher,
                Name = model.Name,
                Title = model.Title,
                Content = model.Content
            };

            await _context.Courses.AddAsync(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Created(nameof(GetById), new { id = course.Id });
            }

            // när jag gör SaveChangesAsync i ett if-uttryck så måste jag här retunera ett felmeddelande som säger "Ett oväntat fel har uppstått"
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, CourseUpdateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna updatera kursen");

            // genom att söka på id kan vi snabbare kontrollerar att kursen inte redan finns i systemet...
            var course = await _context.Courses.FindAsync(id);

            // om course är null då skickas en BadRequest...
            if (course is null) return BadRequest($"Vi kan inte hitta en kurs i systemet med {model.Number}");

            // kontrollera att läraren finns i systemet... 
            var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Teacher.ToUpper().Trim());

            // om läraren inte finns ...
            if (teacher is null) return NotFound($"Vi kunde inte hitta någon lärare med namnet {model.Teacher} i vårt system");


            // flytta över all information i vår modell till UpdateViewModel
            // Put ersätter alla egenskaper med ny information 
            course.Teacher = teacher;
            course.Number = model.Number;
            course.Name = model.Name;
            course.Title = model.Title;
            course.Start = model.Start;
            course.End = model.End;
            course.Content = model.Content;

            // tala om för context och Courses att jag har som syfgte att göra en uyppdatering 
            _context.Courses.Update(course);

            //kontrollera att vi har får tillbaka något som har ändrats 
            if (await _context.SaveChangesAsync() > 0)
            {
                // Gå till databasen och uppdatera en ny kurs...
                return NoContent();
            }

            //annars i värsta fall... 
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("markasfull/{id}")]
        public ActionResult MarkAsFull(int id)
        {
            // Gå till databasen och markera en kurs som fullbokad...
            return NoContent();
        }

        [HttpPatch("markasdone/{id}")]
        public ActionResult MarkAsDone(int id)
        {
            // Gå till databasen och markera en kurs som avklarad...
            return NoContent();
        }
    }
}