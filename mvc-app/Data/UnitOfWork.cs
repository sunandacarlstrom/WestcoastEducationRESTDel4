using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Repository;

namespace WestcoastEducation.Web.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly WestcoastEducationContext _context;
    public UnitOfWork(WestcoastEducationContext context)
    {
        _context = context;
    }

    // delegerar över WestcoastEducationContext till mina repositories
    public IClassroomRepository ClassroomRepository => new ClassroomRepository(_context);

    public IUserRepository UserRepository => new UserRepository(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
