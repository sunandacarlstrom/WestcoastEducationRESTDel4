using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        [HttpGet()]
        public ActionResult List()
        {
            return Ok(new { message = "Lista kurser fungerar" });
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(new { message = $"GetById fungerar {id}" });
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