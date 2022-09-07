using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Presenca;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Interfaces.Business;
public interface IPresencaBusiness
{
    Task<BsnResult<List<BsnRelacaoDeHorasDoColaborador>>> PesquisarAsync(IBsnPesquisaDePresenca bsnPesquisa);
}