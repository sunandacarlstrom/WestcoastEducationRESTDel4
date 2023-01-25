using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/students")]
    public class StudentsController : ControllerBase
    {
        [HttpGet()]
        public ActionResult List()
        {
            return Ok(new { message = "Lista studenter fungerar" });
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(new { message = $"GetById fungerar {id}" });
        }

        [HttpGet("socialsecurityno/{socialSecurityNo}")]
        public ActionResult GetBySocialSecurityNumber(string socialSecurityNo)
        {
            return Ok(new { message = $"GetBySocialSecurityNumber fungerar {socialSecurityNo}" });
        }

        [HttpGet("email/{email}")]
        public ActionResult GetByEmail(string email)
        {
            return Ok(new { message = $"GetByEmail fungerar {email}" });
        }

        [HttpPost()]
        public ActionResult AddStudent()
        {
            // Gå till databasen och lägg till en ny student...
            return Created(nameof(GetById), new { message = "AddStudent fungerar" });
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCourse(int id)
        {
            // Gå till databasen och uppdatera en student...
            return NoContent();
        }

        [HttpPatch("markasapproved/{id}")]
        public ActionResult MarkAsApproved(int id)
        {
            // Gå till databasen och lista kurser som en student är anmäld på...
            return NoContent();
        }

        [HttpPatch("markasdone/{id}")]
        public ActionResult MarkAsDone(int id)
        {
            // Gå till databasen och anmäl en student till nya kurser...
            return NoContent();
        }
    }
}