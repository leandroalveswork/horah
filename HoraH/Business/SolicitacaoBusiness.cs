using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.Bsn.Presenca;
using HoraH.Domain.Models.Bsn.Solicitacao;
using HoraH.Domain.Models.DbModels;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Business;
public class SolicitacaoBusiness : ISolicitacaoBusiness
{
    private readonly IColaboradorLogadoAccessor _colaboradorLogadoAccessor;
    private readonly IColaboradorBusiness _colaboradorBusiness;
    private readonly ISolicitacaoRepository _solicitacaoRepository;
    private readonly IGravadorLogBusiness _gravadorLogBusiness;
    private readonly IUnitOfWork _uow;
    public SolicitacaoBusiness(IColaboradorLogadoAccessor colaboradorLogadoAccessor,
                               IColaboradorBusiness colaboradorBusiness,
                               ISolicitacaoRepository solicitacaoRepository,
                               IGravadorLogBusiness gravadorLogBusiness,
                               IUnitOfWork uow)
    {
        _colaboradorLogadoAccessor = colaboradorLogadoAccessor;
        _colaboradorBusiness = colaboradorBusiness;
        _solicitacaoRepository = solicitacaoRepository;
        _gravadorLogBusiness = gravadorLogBusiness;
        _uow = uow;
    }
    public async Task<BsnResult<List<BsnRelacaoDeSolicitacao>>> PesquisarAsync(BsnPesquisaDeSolicitacao pesquisa, TimeZoneInfo timeZone)
    {
        var resValidacao = pesquisa.Periodo.ValidarRangesEObrigatorios();
        if (!resValidacao.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeSolicitacao>>.Erro(resValidacao.Mensagem);
        }
        var res = BsnResult<List<BsnRelacaoDeSolicitacao>>.Ok;
        var transacaoFoiOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var linqExpFiltro = new LinqExpModel<SolicitacaoDbModel>();
            var filtroSolicitadores = new BsnPesquisaDeColaborador { Nome = pesquisa.NomeSolicitador, EstaAtivo = true };
            var resSolicitadoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroSolicitadores);
            if (!resSolicitadoresRelac.EstaOk)
            {
                res = BsnResult<List<BsnRelacaoDeSolicitacao>>.Erro(resSolicitadoresRelac.Mensagem);
                return;
            }
            if (!string.IsNullOrWhiteSpace(pesquisa.NomeSolicitador))
            {
                var idsSolicitadores = resSolicitadoresRelac.Resultado.Select(x => x.Id);
                linqExpFiltro.AppendAndAlso(x => idsSolicitadores.Contains(x.IdColaboradorSolicitador));
            }
            var filtroAprovadoresReprovadores = new BsnPesquisaDeColaborador { Nome = pesquisa.NomeAprovadorReprovador, EstaAtivo = true };
            var resAprovadoresReprovadoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroAprovadoresReprovadores);
            if (!resAprovadoresReprovadoresRelac.EstaOk)
            {
                res = BsnResult<List<BsnRelacaoDeSolicitacao>>.Erro(resAprovadoresReprovadoresRelac.Mensagem);
                return;
            }
            if (!string.IsNullOrWhiteSpace(pesquisa.NomeAprovadorReprovador))
            {
                var idsAprovadoresReprovadores = resAprovadoresReprovadoresRelac.Resultado.Select(x => x.Id);
                linqExpFiltro.AppendAndAlso(x => x.IdColaboradorAprovadorOuRejeitador != null && x.IdColaboradorAprovadorOuRejeitador != "" && idsAprovadoresReprovadores.Contains(x.IdColaboradorAprovadorOuRejeitador));
            }
            if (!string.IsNullOrEmpty(pesquisa.IdTipoSolicitacao))
            {
                linqExpFiltro.AppendAndAlso(x => x.IdTipoSolicitacao == pesquisa.IdTipoSolicitacao);
            }
            if (!string.IsNullOrEmpty(pesquisa.IdStatusSolicitacao))
            {
                linqExpFiltro.AppendAndAlso(x => x.IdStatusSolicitacao == pesquisa.IdStatusSolicitacao);
            }
            var solicitacoesDb = (await _solicitacaoRepository.SelectByLinqExpModelAsync(linqExpFiltro))
                .Where(x => pesquisa.Periodo.DateEValido(BsnDateTimeModel.FromDb(x.HoraSolicitacao, timeZone).Value))
                .ToList();
            
            await _gravadorLogBusiness.GravarMuitasVisualizacoesAsync(solicitacoesDb, BsnColunaLiterais.ListarIdsColunas("Solicitacao", new [] { "Id", "IdColaboradorSolicitador", "IdTipoSolicitacao", "HoraSolicitacao", "IdStatusSolicitacao", "IdColaboradorAprovadorOuRejeitador" }, typeof(SolicitacaoDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
            res.Resultado = solicitacoesDb.Select(solic => new BsnRelacaoDeSolicitacao
            {
                Id = solic.Id,
                IdSolicitador = solic.IdColaboradorSolicitador,
                NomeSolicitador = resSolicitadoresRelac.Resultado.First(x => x.Id == solic.IdColaboradorSolicitador).Nome,
                IdTipoSolicitacao = solic.IdTipoSolicitacao,
                HoraSolicitacao = BsnDateTimeModel.FromDb(solic.HoraSolicitacao, timeZone).Value,
                IdStatusSolicitacao = solic.IdStatusSolicitacao,
                IdAprovadorReprovador = solic.IdColaboradorAprovadorOuRejeitador,
                NomeAprovadorReprovador = resAprovadoresReprovadoresRelac.Resultado.First(x => x.Id == solic.IdColaboradorAprovadorOuRejeitador).Nome
            }).ToList();
        });
        if (!transacaoFoiOk)
        {
            return BsnResult<List<BsnRelacaoDeSolicitacao>>.Erro(Message.ErroNoServidor);
        }
        return res;
    }
}