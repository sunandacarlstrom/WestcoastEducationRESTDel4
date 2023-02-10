using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;
using WestcoastEducation.Api.ViewModels.Students;

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
        public async Task<ActionResult> ListAll()
        {
            var result = await _context.Students
            .Select(s => new StudentListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _context.Students
            .Include(c => c.Course)
            .Select(s => new StudentDetailsViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Course = s.Course!.Name ?? "",
                Email = s.Email
            })
            .SingleOrDefaultAsync(s => s.Id == id);
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            var result = await _context.Students
                .Include(c => c.Course)
                // sätter ett villkor som beskriver vad jag vill söka på
                .Where(s => s.Email!.ToUpper().Trim() == email.ToUpper().Trim())
                .Select(s => new StudentDetailsViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Course = s.Course!.Name ?? "",
                    Email = s.Email
                })
            .ToListAsync();
            return Ok(result);
        }

        [HttpPost()]
        public async Task<ActionResult> AddStudent(StudentAddListViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna uppdatera kursen");

            // kontrollerar om studenten redan finns i systemet     
            var exists = await _context.Students.SingleOrDefaultAsync(s =>
            s.Email == model.Email);

            if (exists is not null) return BadRequest($"Studenten med e-post {model.Email} finns redan i systemet");

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Course.ToUpper().Trim());
            if (course is null) return NotFound($"Vi kunde inte hitta en kurs med namnet {model.Course} i vårt system");

            var student = new StudentModel
            {
                Name = model.Name,
                Email = model.Email,
                Course = course
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

        public async Task<ActionResult> UpdateStudent(int id, StudentUpdateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna uppdatera kursen");

            var student = await _context.Students.FindAsync(id);

            if (student is null) return NotFound($"Vi kunde inte hitta en student med {model.Name} i vårt system");

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Course.ToUpper().Trim());

            // Put ersätter/uppdaterar alla egenskaper med ny information 
            student.Course = course;
            student.Name = model.Name;
            student.Email = model.Email;

            // tala om för context och Student att jag har som syfte att göra en uppdatering
            _context.Students.Update(student);

            //kontrollera att jag har får tillbaka något som har ändrats 
            if (await _context.SaveChangesAsync() > 0)
            {
                // Gå till databasen och uppdatera en student...
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addcourse/{studentId}")]
        public async Task<ActionResult> AddCourse(int studentId, StudentAddCourseViewModel model)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student is null) return NotFound($"Tyvärr kunde vi inte hitta någon student med id {studentId}");

            var course = await _context.Courses.FindAsync(model.CourseId);
            if (course is null) return NotFound($"Tyvärr kunde vi inte hitta någon kurs med id {model.CourseId}");

            if (course.Students is null) course.Students = new List<StudentModel>();

            course.Students.Add(student);

            _context.Update(course);

            //kontrollera att jag har får tillbaka något som har ändrats 
            if (await _context.SaveChangesAsync() > 0)
            {
                // Gå till databasen och anmäl en student till nya kurser...
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("withdraw/{studentId}")]
        public async Task<ActionResult> Withdraw(int studentId)
        {
            //hämta in kursen som jag vill radera hos en student
            var student = await _context.Students.FindAsync(studentId);
            if (student is null) return NotFound($"Student med Id {studentId} kunde inte hittas");

            var course = await _context.Courses.FindAsync(student.CourseId);
            if (course is null) return NotFound("Studenten är inte anmäld på någon kurs");

            course.Students!.Remove(student);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        // //TODO: Vill egentligen ta bort endast kursen från en student, inte hela studenten. Har ännu inte lärt oss Many-To-Many förhållande.
        // [HttpDelete("delete/{studentId}")]
        // public async Task<ActionResult> Delete(int studentId)
        // {
        //     //hämta in kursen som jag vill radera hos en student
        //     var student = await _context.Students.FindAsync(studentId);
        //     if (student is null) return NotFound($"Student med Id {studentId} kunde inte hittas");

        //     var course = await _context.Courses.FindAsync(student.CourseId);
        //     if (course is null) return NotFound("Studenten är inte anmäld på någon kurs");

        //     course.Students!.Remove(student);
        //     if (await _context.SaveChangesAsync() > 0)
        //     {
        //         return NoContent();
        //     }

        //     return StatusCode(500, "Internal Server Error");
        // }
    }
}