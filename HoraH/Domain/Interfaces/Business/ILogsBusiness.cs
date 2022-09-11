using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Logs;

namespace HoraH.Domain.Interfaces.Business;
public interface ILogsBusiness
{
    Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarAsync(BsnPesquisaDeLogs pesquisa, TimeZoneInfo timeZone);
    Task<BsnResult<BsnRelacaoDeLog>> ObterPorIdAsync(string idOperacao, string idRegistro, TimeZoneInfo timeZone);
}