namespace WestcoastEducation.Web.Interfaces;

public interface IUnitOfWork
{
    //ansvarar för att spara ner alla förändringar till databasen
    IClassroomRepository ClassroomRepository { get; }
    IUserRepository UserRepository { get; }

    Task<bool> Complete();
}
