using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;

namespace WestcoastEducationRESTDel1.api.Controllers
{
    [ApiController]
    [Route("api/v1/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public TeachersController(WestcoastEducationContext context)
        {
            _context = context;
        }

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
        public async Task<ActionResult> AddTeacher(TeacherAddViewModel model)
        {
            var teacher = new TeacherModel
            {
                Name = model.Name,
                Email = model.Email,
                Skills = new List<TeacherSkillsModel>() //Skapar en listan för lärarskills.Lägger till skills 2 rader nedanför
            };

            // loopar igenom listan med skills som man har lagt till och lägger till dessa i läraren 
            foreach (var item in model.TeacherSkills)
            {
                var skill = await _context.TeacherSkills.SingleOrDefaultAsync(c => c.Skill == item.Skill);

                if (skill is not null)
                {
                    teacher.Skills.Add(skill);
                }
            }

            await _context.Teachers.AddAsync(teacher);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { Id = teacher.Id }, new
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    Email = teacher.Email,
                    Skill = teacher.Skills
                });
            }

            return StatusCode(500, "Internal Server Error");
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

        [HttpPatch("add-course-to-teacher/{id}")]
        public ActionResult AddCourseToTeacher(int id)
        {
            // Gå till databasen och lägg till kurs som lärare kan undervisa i...
            return NoContent();
        }
    }
}