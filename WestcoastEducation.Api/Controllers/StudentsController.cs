using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/students")]
    public class StudentsController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public StudentsController(WestcoastEducationContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult> List()
        {
            var result = await _context.Students
            .Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }
            ).ToListAsync();

            return Ok(result);
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
        public async Task<ActionResult> AddStudent(StudentAddListViewModel model)
        {
            // kontrollerar om studenten redan finns i systemet     
            var exists = await _context.Students.SingleOrDefaultAsync(s =>
            s.Email == model.Email);

            if (exists is not null) return BadRequest($"Studenten med e-post {model.Email} finns redan i systemet");

            var student = new StudentModel
            {
                Name = model.Name,
                Email = model.Email,
                CourseId = model.CourseId
            };

            await _context.Students.AddAsync(student);

            if (await _context.SaveChangesAsync() > 0)
            {
                // Gå till databasen och lägg till en ny student...
                return Created(nameof(GetById), new { id = student.Id });
            }

            // när jag gör SaveChangesAsync i ett if-uttryck så måste jag här retunera ett felmeddelande som säger "Ett oväntat fel har uppstått"
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id)
        {
            // Gå till databasen och uppdatera en student...
            return NoContent();
        }

        [HttpGet("list-approved-courses/{id}")]
        public async Task<ActionResult> ListApprovedCourses(int id)
        {
            // Gå till databasen och lista kurser som en student är anmäld på...
            return Ok(new { message = $"Lista kurser som en student är anmäld på fungerar {id}" });
        }

        [HttpPatch("add-course-to-student/{id}")]
        public async Task<ActionResult> AddCourseToStudent(int courseId, int studentId)
        {
            var student = await _context.Students.FindAsync(studentId); 
            if (student is null) return NotFound($"Tyvärr kunde vi inte hitta någon student med id {studentId}");

            var course = await _context.Courses.FindAsync(courseId); 
            if (course is null) return NotFound($"Tyvärr kunde vi inte hitta någon kurs med id {courseId}"); 

            if(course.Students is null) course.Students = new List<StudentModel>(); 

            course.Students.Add(student); 

            _context.Update(course); 
 
            
            // Gå till databasen och anmäl en student till nya kurser...
            return NoContent();
        }
    }
}