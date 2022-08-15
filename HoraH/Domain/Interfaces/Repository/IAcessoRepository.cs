using HoraH.Domain.Interfaces.Repository.Common;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Interfaces.Repository;
public interface IAcessoRepository : IRepositoryBase<AcessoDbModel>
{
    Task<List<AcessoDbModel>> SelectByIdDoColaboradorAsync(string idDoColaborador);
    // Acessos padrão podem apenas marcar presença e solicitar
    List<AcessoDbModel> MontarAcessosPadraoParaColaborador(string idColaborador);
}