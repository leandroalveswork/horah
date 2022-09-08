using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Logs;

namespace HoraH.Business;
public class LogsBusiness : ILogsBusiness
{
    public async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarAsync(BsnPesquisaDeLogs pesquisa)
    {
        throw new NotImplementedException();
    }
}