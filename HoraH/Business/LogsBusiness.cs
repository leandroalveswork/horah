using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.DbModels;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Business;
public class LogsBusiness : ILogsBusiness
{
    private readonly IColaboradorBusiness _colaboradorBusiness;
    public LogsBusiness(IColaboradorBusiness colaboradorBusiness)
    {
        _colaboradorBusiness = colaboradorBusiness;
    }

    public async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarAsync(BsnPesquisaDeLogs pesquisa, TimeZoneInfo timeZone)
    {
        var resValid = pesquisa.ValidarRangeHoras();
        if (!resValid.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeLog>>.Erro(resValid.Mensagem);
        }
        var lConteudoRes = new List<BsnRelacaoDeLog>();
        throw new NotImplementedException();
        
    }

    public async Task<BsnResult<BsnRelacaoDeLog>> ObterPorIdAsync(string idOperacao, string idRegistro, TimeZoneInfo timeZone)
    {
        throw new NotImplementedException();
    }
}