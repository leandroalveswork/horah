namespace HoraH.Domain.Interfaces.Repository.Common;
public interface IRepositoryBase<T>
{
    Task<List<T>> SelectAllAsync();
    Task<T?> SelectByIdAsync(string id);
    Task InsertAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}