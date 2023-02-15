using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Interfaces;

//här sätter jag upp metoder som ska jobba mot databasen 
public interface IUserRepository : IRepository<UserModel>
{
    Task<UserModel?> FindByEmailAsync(string mail);
}
