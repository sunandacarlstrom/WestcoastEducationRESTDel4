using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Interfaces;

//här sätter jag upp metoder som ska jobba mot databasen 
public interface IClassroomRepository : IRepository<ClassroomModel>
{
    Task<ClassroomModel?> FindByNumberAsync(string numb);
}
