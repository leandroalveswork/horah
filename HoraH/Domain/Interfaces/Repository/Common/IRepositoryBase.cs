using System.Linq.Expressions;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Domain.Interfaces.Repository.Common;
public interface IRepositoryBase<T>
{
    Task<List<T>> SelectAllAsync();
    Task<T?> SelectByIdAsync(string id);
    Task<List<T>> SelectByLinqExpModelAsync(LinqExpModel<T> linqExpMd);
    Task InsertAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}