using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Repository;

public class UserRepository : Repository<UserModel>, IUserRepository
{
    // definerar att det är UserRepository som kommunicerar med databasen via föräldrarklassen Repository
    public UserRepository(WestcoastEducationContext context) : base(context) { }

    public async Task<UserModel?> FindByEmailAsync(string mail)
    {
        // tillåter INTE dubletter av e-postadresser med metoden SingleOrDefaultAsync
        return await _context.Users.SingleOrDefaultAsync(u => u.Email.Trim().ToLower() ==
        mail.Trim().ToLower());
    }
}
