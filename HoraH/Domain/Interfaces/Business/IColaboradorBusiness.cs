using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Common;

namespace HoraH.Domain.Interfaces.Business;
public interface IColaboradorBusiness
{
    Task<BsnResult<List<BsnRelacaoDeColaborador>>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa);
    Task<BsnResult<object>> AtivarAsync(string id);
    Task<BsnResult<object>> InativarAsync(string id);
}