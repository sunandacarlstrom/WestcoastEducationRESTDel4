using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Api.Data;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.ViewModels;
using WestcoastEducation.Api.ViewModels.TeacherSkills;

namespace WestcoastEducation.Api.Controllers;

[ApiController]
[Route("api/v1/teacherskills")]
public class TeacherSkillsController : ControllerBase
{
    private readonly WestcoastEducationContext _context;
    public TeacherSkillsController(WestcoastEducationContext context)
    {
        _context = context;
    }

    [HttpGet("listall")]
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

    [HttpGet("getbyid/{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _context.TeacherSkills
        .Select(k => new TeacherSkillsDetailsViewModel
        {
            Id = k.Id,
            Skill = k.Skill,
            TeacherName = k.Teacher.Name ?? "Ej tilldelad"
        })
       // jag vill ha tag i ett Id som stämmer överrens med det Id som jag skickar in 
       .SingleOrDefaultAsync(c => c.Id == id);
        return Ok(result);
    }

    [HttpGet("{skill}/teachers")]
    public async Task<ActionResult> ListTeacherWithSkill(string skill)
    {
        var result = await _context.TeacherSkills
        .Select(k => new
        {
            Id = k.Id,
            Skill = k.Skill,
            Teacher = new
            {
                Id = k.Teacher.Id,
                Name = $"{k.Teacher.Name} {k.Teacher.Email}"
            }
        }).SingleOrDefaultAsync(s => s.Skill.ToUpper().Trim() == skill.ToUpper().Trim());

        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult> AddSkill(TeacherSkillsAddViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna lägga till kompetensområdet i systemet");

        var exists = await _context.TeacherSkills.SingleOrDefaultAsync(s => s.Skill.ToUpper().Trim() == model.Skill.ToUpper().Trim());
        if (exists is not null) return BadRequest($"Vi har redan registrerat en kompetens med namnet {model.Skill}");

        var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == model.TeacherId);
        // säger också att man inte behöver skicka med ett TeacherId ifall man endast vill lägga till en kompetens utan koppling till en specifik lärare 
        if (teacher is null && model.TeacherId is not null) return NotFound($"Vi kunde inte hitta någon lärare med id {model.TeacherId} i vårt system");

        var skill = new TeacherSkillsModel
        {
            Teacher = teacher,
            Skill = model.Skill
        };

        await _context.TeacherSkills.AddAsync(skill);

        if (await _context.SaveChangesAsync() > 0)
        {
            return Created(nameof(GetById), new { id = skill.Id });
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSkill(int id, TeacherSkillsAddViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna uppdatera kompetensområdet i systemet");

        var exists = await _context.TeacherSkills.SingleOrDefaultAsync(s => s.Skill.ToUpper().Trim() == model.Skill.ToUpper().Trim());
        if (exists is not null) return BadRequest($"Vi har redan registrerat en kompetens med namnet {model.Skill}");

        var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == model.TeacherId);
        // säger också att man inte behöver skicka med ett TeacherId ifall man endast vill lägga till en kompetens utan koppling till en specifik lärare 
        if (teacher is null && model.TeacherId is not null) return NotFound($"Vi kunde inte hitta någon lärare med id {model.TeacherId} i vårt system");

        var teacherSkills = await _context.TeacherSkills.SingleOrDefaultAsync(ts => ts.Id == id);
        if (teacherSkills is null) return NotFound($"Vi kunde inte hitta någon kompetens med id {id} i vårt system");

        teacherSkills.Skill = model.Skill;
        teacherSkills.Teacher = teacher;

        _context.TeacherSkills.Update(teacherSkills);

        if (await _context.SaveChangesAsync() > 0)
        {
            return Created(nameof(GetById), new { id = teacherSkills.Id });
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("setskilltoteacher/{id}/{teacherId}")]
    public async Task<ActionResult> SetSkillToTeacher(int id, int teacherId)
    {
        var teacherSkill = await _context.TeacherSkills.SingleOrDefaultAsync(s => s.Id == id);
        if (teacherSkill is null) return BadRequest($"Vi hittar inte en kompetens med id {id}");

        var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == teacherId);
        if (teacher is null) return NotFound($"Vi kunde inte hitta någon lärare med id {teacherId} i vårt system");

        teacherSkill.Teacher = teacher;

        _context.TeacherSkills.Update(teacherSkill);

        if (await _context.SaveChangesAsync() > 0)
        {
            return Created(nameof(GetById), new { Id = teacherSkill.Id, Skill = teacherSkill.Skill, Teacher = teacherSkill.Teacher.Name });
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSkill(int id)
    {
        var teacherSkill = await _context.TeacherSkills.FindAsync(id);
        if (teacherSkill is null) return NotFound($"Kompetens med id {id} kunde inte hittas");

        _context.TeacherSkills.Remove(teacherSkill);
        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }
}