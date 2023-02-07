using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;

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
        public async Task<ActionResult> List()
        {
            // var skill = await _context.TeacherSkills
            // .Include(s => s.Teacher).SingleOrDefaultAsync(c => c.Id == id);

            // if (skill is null) return NotFound($"Vi kunde inte hitta någon kompetens med id {id}");

            // var result = new
            // {
            //     Id = skill.Id,
            //     Name = skill.Name,
            //     Teacher = skill.Teacher?.Select(t => new
            //     {
            //         Name = t.Name,
            //         Email = t.Email
            //     }).ToList()
            // };
            return Ok();
        }

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


    }
}