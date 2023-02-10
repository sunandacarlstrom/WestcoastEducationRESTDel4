using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;
using WestcoastEducation.Api.ViewModels.TeacherSkills;

namespace WestcoastEducation.Api.Controllers
{
    [ApiController]
    [Route("api/v1/teacherskills")]
    public class TeacherSkillsController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public TeacherSkillsController(WestcoastEducationContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult> ListAll()
        {
            var result = await _context.TeacherSkills
            .Include(t => t.Teacher)
            .Select(k => new TeacherSkillsListViewModel
            {
                Id = k.Id,
                Skill = k.Skill
            })
            // listar alla skills
            .ToListAsync();
            return Ok(result);
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult> GetById(int id)
        // {

        //     var result = await _context.TeacherSkills
        //     .Include(t => t.Teacher)
        //     .Select(k => new TeacherSkillsDetailsViewModel
        //     {
        //         Id = k.Id,
        //         Skill = k.Skill,
        //         Teachers = k.Teachers.Select(s => new TeacherListViewModel
        //         {
        //             Id = s.Id,
        //             Name = s.Name
        //         }).ToList()
        //     })
        //    // jag vill ha tag i ett Id som stämmer överrens med det Id som jag skickar in 
        //    .SingleOrDefaultAsync(c => c.Id == id);
        //     return Ok(result);
        // }

        // [HttpPost("addskill")]
        // public async Task<ActionResult> AddSkill(TeacherAddViewModel model)
        // {
        //     if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna lagra kompetensområden i systemet");

        //     // kontrollerar att kompetensen inte redan finns i systemet...
        //     var exists = await _context.TeacherSkills.SingleOrDefaultAsync(c => c.Number!.ToUpper().Trim() == model.Number!.ToUpper().Trim());

        //     // om exits inte är null då skickas en BadRequest...
        //     if (exists is not null) return BadRequest($"Vi har redan registrerat en kurs med kursnummer {model.Number}");

        //     // kontrollera att läraren finns i systemet... 
        //     var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Name!.ToUpper().Trim() == model.Teacher.ToUpper().Trim());

        //     // om läraren inte finns ...
        //     if (teacher is null) return NotFound($"Vi kunde inte hitta någon lärare med namnet {model.Teacher} i vårt system");

        //     // skapar CourseModel som ska skickas till databasen 
        //     var skill = new TeacherSkillsModel
        //     {
        //         Id = s.Id,
        //         Name = s.Name
        //     };

        //     await _context.TeacherSkills.AddAsync(skill);

        //     if (await _context.SaveChangesAsync() > 0)
        //     {
        //         return Created(nameof(GetById), new { id = skill.Id });
        //     }

        //     return StatusCode(500, "Internal Server Error");
        // }


        [HttpPatch("set-skill-to-teacher/{id}")]
        public async Task<ActionResult> SetSkillToTeacher(int id, string skillName)
        {
            // var exists = await _context.TeacherSkills.SingleOrDefaultAsync(c => c.Name.ToUpper() == skillName.ToUpper());

            // if (exists is not null) return BadRequest($"Kompetens {skillName} finns redan i systemet");

            // var skill = new { Name = skillName };

            // await _context.TeacherSkills.AddAsync(skill);

            // if (await _context.SaveChangesAsync() > 0)
            // {
            //     return CreatedAtAction(nameof(GetById), new { Id = skill.Id, Name = skill.Name });

            // }

            return StatusCode(500, "Internal Server Error");
        }

        // [HttpPatch("withdraw/{teacherId}")]
        // public async Task<ActionResult> DeleteSkill(int teacherId)
        // {
        //     // var teacher = await _context.Teachers.FindAsync(teacherId);

        //     // if (teacher is null) return NotFound($"Vi kan inte hitta någon lärare med id: {teacherId}");

        //     // var skill = await _context.TeacherSkills.FindAsync(teacher.TeacherId);
        //     // if (skill is null) return NotFound("Läraren har inga angivna skills");

        //     // skill.Teachers!.Remove(skill);
        //     // if (await _context.SaveChangesAsync() > 0)
        //     // {
        //     //     return NoContent();
        //     // }

        //     return StatusCode(500, "Internal Server Error");
        // }
    }
}