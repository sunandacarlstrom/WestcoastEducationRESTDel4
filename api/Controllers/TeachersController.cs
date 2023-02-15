using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;
using WestcoastEducation.Api.ViewModels.Teachers;

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

        [HttpGet("listall")]
        public async Task<ActionResult> ListAll()
        {
            var result = await _context.Teachers
            .Select(t => new TeacherListViewModel
            {
                Id = t.Id,
                Name = t.Name,
            })
            // listar alla lärare
            .ToListAsync();
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _context.Teachers
            .Include(c => c.Courses)
            .Include(s => s.Skills)
            .Select(t => new TeacherDetailsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Courses = t.Courses.Select(c => new CourseListViewModel
                {
                    Id = c.Id,
                    Teacher = c.Teacher.Name ?? "",
                    Number = c.Number,
                    Name = c.Name,
                    Title = c.Title
                }).ToList(),
                Skills = t.Skills.Select(s => new TeacherSkillsListViewModel
                {
                    Id = s.Id,
                    Skill = s.Skill
                }).ToList()
            })
            // jag vill ha tag i ett Id som stämmer överrens med det Id som jag skickar in 
            .SingleOrDefaultAsync(c => c.Id == id);
            return Ok(result);
        }

        [HttpGet("getbyemail/{email}")]
        public async Task<ActionResult> GetByEmail(string email)
        {
            var result = await _context.Teachers
            .Include(c => c.Courses)
            .Include(s => s.Skills)
            .Where(s => s.Email.ToUpper().Trim() == email.ToUpper().Trim())
            .Select(t => new TeacherDetailsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Courses = t.Courses.Select(c => new CourseListViewModel
                {
                    Id = c.Id,
                    Teacher = c.Teacher.Name ?? "",
                    Number = c.Number,
                    Name = c.Name,
                    Title = c.Title
                }).ToList(),
                Skills = t.Skills.Select(s => new TeacherSkillsListViewModel
                {
                    Id = s.Id,
                    Skill = s.Skill
                }).ToList()
            })
            .ToListAsync();
            // jag vill ha tag i ett Id som stämmer överrens med det Id som jag skickar in 
            return Ok(result);
        }

        [HttpPost()]
        public async Task<ActionResult> AddTeacher(TeacherAddViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna lägga till en lärare");

            var teacher = new TeacherModel
            {
                Name = model.Name,
                Email = model.Email,
                Courses = new List<CourseModel>(),

                // Skapar en lista för lärarskills. Lägger till skills 2 rader nedanför
                Skills = new List<TeacherSkillsModel>()
            };

            // loopar igenom listan med skills som man har lagt till och lägger till dessa i läraren 
            foreach (var courseId in model.CourseIds)
            {
                var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);

                if (course is not null)
                {
                    teacher.Courses.Add(course);
                }
            }

            // loopar igenom listan med skills som man har lagt till och lägger till dessa i läraren 
            foreach (var skillId in model.TeacherSkillIds)
            {
                var skill = await _context.TeacherSkills.SingleOrDefaultAsync(c => c.Id == skillId);

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
                    Email = teacher.Email
                });
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeacher(int id, TeacherUpdateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna uppdatera läraren");

            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == id);
            if (teacher is null) return NotFound($"Vi kunde inte hitta en lärare med id {id} i vårt system");

            teacher.Name = model.Name;
            teacher.Email = model.Email;

            teacher.Courses = new List<CourseModel>();

            // loopar igenom listan med skills som man har lagt till och lägger till dessa i läraren 
            foreach (var courseId in model.CourseIds)
            {
                var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);

                if (course is not null)
                {
                    teacher.Courses.Add(course);
                }
            }

            teacher.Skills = new List<TeacherSkillsModel>();

            // loopar igenom listan med skills som man har lagt till och lägger till dessa i läraren 
            foreach (var skillId in model.TeacherSkillIds)
            {
                var skill = await _context.TeacherSkills.SingleOrDefaultAsync(c => c.Id == skillId);

                if (skill is not null)
                {
                    teacher.Skills.Add(skill);
                }
            }

            _context.Teachers.Update(teacher);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetById), new { Id = teacher.Id }, new
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    Email = teacher.Email
                });
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("setcompetence/{teacherId}")]
        public async Task<ActionResult> SetCompetence(int teacherId, TeacherAddCompetenceViewModel model)
        {
            var teacher = await _context.Teachers
            .SingleOrDefaultAsync(t => t.Id == teacherId);

            if (teacher is null) return BadRequest($"Vi kunde inte hitta läraren med id {teacherId}");

            var competence = await _context.TeacherSkills.FindAsync(model.Id);
            if (competence is null) return NotFound($"Tyvärr kunde vi inte hitta någon kompetens med id {model.Id}");

            // Om inte är initierad skapas en lista för det inte ska blir null när man lägger till kompetens nedanför
            // eftersom en lärare kan ha flera skills
            if (teacher.Skills is null) teacher.Skills = new List<TeacherSkillsModel>();

            teacher.Skills.Add(competence);

            _context.Update(teacher);

            //kontrollera att jag har får tillbaka något som har ändrats 
            if (await _context.SaveChangesAsync() > 0)
            {
                // Gå till databasen och uppdatera en lärare...
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("setcourse/{teacherId}")]
        public async Task<ActionResult> SetCourse(int teacherId, TeacherAddCourseViewModel model)
        {
            var teacher = await _context.Teachers
            .SingleOrDefaultAsync(t => t.Id == teacherId);

            if (teacher is null) return BadRequest($"Vi kunde inte hitta läraren med id {teacherId}");

            var course = await _context.Courses.FindAsync(model.Id);
            if (course is null) return NotFound($"Tyvärr kunde vi inte hitta någon kurs med id {model.Id}");

            //Om inte är initzerard skapas en lista för det inte ska blir null när man lägger till kompetens nedanför
            //efter sopm en lärare kan ha flera skills
            if (teacher.Courses is null) teacher.Courses = new List<CourseModel>();

            teacher.Courses.Add(course);

            _context.Update(teacher);

            //kontrollera att jag har får tillbaka något som har ändrats 
            if (await _context.SaveChangesAsync() > 0)
            {
                // Gå till databasen och uppdatera en lärare...
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }
    }
}