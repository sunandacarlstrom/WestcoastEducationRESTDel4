namespace WestcoastEducation.Web.Interfaces;

// T representerar endast klasserna Classroom eller User 
public interface IRepository<T> where T : class
{
    Task<IList<T>> ListAllAsync();
    Task<T?> FindByIdAsync(int id);
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}
