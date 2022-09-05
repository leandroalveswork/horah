using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Models.Bsn.Colaborador;

namespace HoraH.Domain.Interfaces.Business;
public interface IColaboradorBusiness
{
    Task<BsnResult<List<BsnRelacaoDeColaborador>>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa);
    Task<BsnResult<object>> AtivarAsync(string id);
    Task<BsnResult<object>> InativarAsync(string id);
    Task<BsnResult<BsnRelacaoDeColaborador>> ObterPorIdAsync(string id);
    Task<BsnResult<List<BsnAcesso>>> ObterAcessosDoColaboradorAsync(string idColaborador);
    Task<BsnResult<object>> AlterarAcessosAsync(string idColaborador, List<BsnAcesso> acessos);
}