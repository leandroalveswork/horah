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
    public async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarAsync(BsnPesquisaDeLogs pesquisa)
    {
        // var resValidacao = pesquisa.ValidarRangeHoras();
        // if (!resValidacao.EstaOk)
        // {
        //     return BsnResult<List<BsnRelacaoDeLog>>.Erro(resValidacao.Mensagem);
        // }
        // var linqExpFiltro = new LinqExpModel<RegistroDbModel>();
        // var filtroColaboradores = new BsnPesquisaDeColaborador { Nome = pesquisa.NomeColaborador, EstaAtivo = true };
        // var resColaboradoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroColaboradores);
        // if (!resColaboradoresRelac.EstaOk)
        // {
        //     return BsnResult<List<BsnRelacaoDeLog>>.Erro(resColaboradoresRelac.Mensagem);
        // }
        // if (!string.IsNullOrWhiteSpace(pesquisa.NomeColaborador))
        // {
        //     var idsColaboradores = resColaboradoresRelac.Resultado.Select(x => x.Id).ToList();
        //     linqExpFiltro.AppendAndAlso(x => idsColaboradores.Contains(x.IdColaborador));
        // }
        // if (pesquisa.IdEvento != null)
        // {
        //     linqExpFiltro.AppendAndAlso(x => x.IdEvento == bsnPesquisa.IdEvento);
        // }
        throw new NotImplementedException();
    }
}