using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Repository;

public class ClassroomRepository : Repository<ClassroomModel>, IClassroomRepository
{
    // definerar att det är ClassroomRepository som kommunicerar med databasen via föräldrarklassen Repository
    public ClassroomRepository(WestcoastEducationContext context) : base(context){}

    public async Task<ClassroomModel?> FindByNumberAsync(string numb)
    {
        return await _context.Classrooms.SingleOrDefaultAsync(c => c.Number.Trim().ToLower() == numb.Trim().ToLower());
    }
}
