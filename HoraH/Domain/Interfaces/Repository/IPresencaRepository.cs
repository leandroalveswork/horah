using HoraH.Domain.Interfaces.Repository.Common;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Interfaces.Repository;
public interface IPresencaRepository : IRepositoryBase<PresencaDbModel>
{
    Task<List<PresencaDbModel>> SelectPorEventoAsync(string idEvento);
    Task<List<PresencaDbModel>> SelectEntreColaboradoresEPorEventoAsync(List<string> idsColaborador, string idEvento);
}