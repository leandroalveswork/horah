using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Solicitacao;

namespace HoraH.Domain.Interfaces.Business;
public interface ISolicitacaoBusiness
{
    Task<BsnResult<List<BsnRelacaoDeSolicitacao>>> PesquisarAsync(BsnPesquisaDeSolicitacao pesquisa, TimeZoneInfo timeZone);
}