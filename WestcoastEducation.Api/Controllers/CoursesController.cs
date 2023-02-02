using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
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
                Title = c.Title, 
                Content = c.Content
            })
            .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _context.Courses.FindAsync(id);
            return Ok(result);
        }

        [HttpGet("courseno/{courseNo}")]
        public ActionResult GetByCourseNumber(string courseNo)
        {
            return Ok(new { message = $"GetByCourseNumber fungerar {courseNo}" });
        }

        [HttpGet("coursetitle/{courseTitle}")]
        public ActionResult GetByCourseTitle(string courseTitle)
        {
            return Ok(new { message = $"GetByCourseTitle fungerar {courseTitle}" });
        }

        [HttpGet("coursestart/{courseStart}")]
        public ActionResult GetByCourseStart(string courseStart)
        {
            return Ok(new { message = $"GetByCourseStart fungerar {courseStart}" });
        }

        [HttpPost()]
        public ActionResult AddCourse()
        {
            // Gå till databasen och lägg till en ny kurs...
            return Created(nameof(GetById), new { message = "AddCourse fungerar" });
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCourse(int id)
        {
            // Gå till databasen och uppdatera en ny kurs...
            return NoContent();
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