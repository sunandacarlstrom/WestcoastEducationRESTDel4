using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/teachers")]
    public class TeachersController : ControllerBase
    {
        [HttpGet()]
        public ActionResult List()
        {
            return Ok(new { message = "Lista lärare fungerar" });
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(new { message = $"GetById fungerar {id}" });
        }

        [HttpGet("email/{email}")]
        public ActionResult GetByEmail(string email)
        {
            return Ok(new { message = $"GetByEmail fungerar {email}" });
        }

        [HttpPost()]
        public ActionResult AddTeacher()
        {
            // Gå till databasen och lägg till en ny lärare...
            return Created(nameof(GetById), new { message = "AddTeacher fungerar" });
        }

        [HttpPut("{id}")]
        public ActionResult UpdateTeacher(int id)
        {
            // TODO: Lägg till ett nytt kompetensområde

            // Gå till databasen och uppdatera en lärare...
            return NoContent();
        }

        [HttpGet("list-assigned-courses/{id}")]
        public ActionResult ListAssignedCourses(int id)
        {
            // Gå till databasen och lista vilka kurser som en lärare undervisar...
            return Ok(new { message = $"Lista vilka kurser som en lärare undervisar fungerar {id}" });
        }

        [HttpPatch("add-teacher-to-course/{id}")]
        public ActionResult AddTeacherToCourse(int id)
        {
            // Gå till databasen och lägg till kurs som lärare kan undervisa i...
            return NoContent();
        }
    }
}