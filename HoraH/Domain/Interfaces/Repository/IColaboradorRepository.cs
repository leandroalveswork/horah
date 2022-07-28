using HoraH.Domain.Interfaces.Repository.Common;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Interfaces.Repository;
public interface IColaboradorRepository : IRepositoryBase<ColaboradorDbModel>
{
    Task<ColaboradorDbModel?> SelectByLoginAsync(string login);
}