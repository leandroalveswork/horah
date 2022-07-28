using HoraH.Domain.Interfaces.Repository.Common;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Interfaces.Repository;
public interface IFuncionalidadeRepository : IRepositoryBase<FuncionalidadeDbModel>
{
    List<FuncionalidadeDbModel> ListarFuncionalidadesDoSistema();
    string GetNomeDaFuncionalidadeComId(string? id);
    string GetIdDaFuncionalidadeComNome(string nome);
}