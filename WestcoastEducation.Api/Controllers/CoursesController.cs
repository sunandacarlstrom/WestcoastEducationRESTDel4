using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        //kontakt med databasen 
        private readonly WestcoastEducationContext _context;
        public CoursesController(WestcoastEducationContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult> List()
        {
            var result = await _context.Courses.ToListAsync();
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