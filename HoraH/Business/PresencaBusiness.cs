using HoraH.Domain.Design.Patterns.Strategy;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Evento;
using HoraH.Domain.Models.Bsn.Presenca;
using HoraH.Domain.Models.DbModels;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Business;
public class PresencaBusiness : IPresencaBusiness
{
    private readonly IPresencaRepository _presencaRepository;
    private readonly IColaboradorBusiness _colaboradorBusiness;
    public PresencaBusiness(IPresencaRepository presencaRepository, IColaboradorBusiness colaboradorBusiness)
    {
        _presencaRepository = presencaRepository;
        _colaboradorBusiness = colaboradorBusiness;
    }

    public async Task<BsnResult<List<BsnRelacaoDeHorasDoColaborador>>> PesquisarAsync(IBsnPesquisaDePresenca bsnPesquisa)
    {
        var linqExpFiltro = new LinqExpModel<PresencaDbModel>();
        var filtroColaboradores = new BsnPesquisaDeColaborador { Nome = bsnPesquisa.NomeColaborador, EstaAtivo = true };
        var resColaboradoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroColaboradores);
        if (!resColaboradoresRelac.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeHorasDoColaborador>>.Erro(resColaboradoresRelac.Mensagem);
        }
        if (!string.IsNullOrWhiteSpace(bsnPesquisa.NomeColaborador))
        {
            var idsColaboradores = resColaboradoresRelac.Resultado.Select(x => x.Id).ToList();
            linqExpFiltro.AppendAndAlso(x => idsColaboradores.Contains(x.IdColaborador));
        }
        if (bsnPesquisa.IdEvento != null)
        {
            linqExpFiltro.AppendAndAlso(x => x.IdEvento == bsnPesquisa.IdEvento);
        }
        var presencasDb = await _presencaRepository.SelectByLinqExpModelAsync(linqExpFiltro);
        var gruposPresencasPorColaborador = presencasDb.GroupBy(x => x.IdColaborador);
        var relacoesHorasTrabalhadas = new List<BsnRelacaoDeHorasDoColaborador>();
        foreach (var iGrupoPresencas in gruposPresencasPorColaborador)
        {
            var intervalosExpediente = BsnIntervaloDeTempo.ObterIntervalosExpediente(iGrupoPresencas.ToList());
            var intervalosStop = BsnIntervaloDeTempo.ObterIntervalosStop(iGrupoPresencas.ToList());
            var diasRelacao = intervalosExpediente.SelectMany(x => new [] { x.Inicio.Date, x.Fim.Date })
                .Concat(intervalosStop.SelectMany(x => new [] { x.Inicio.Date, x.Fim.Date }))
                .Distinct()
                .Where(x => bsnPesquisa.DateEValido(x));
            var iRelacaoHorasTrabalhadas = new BsnRelacaoDeHorasDoColaborador
            {
                IdColaborador = iGrupoPresencas.Key,
                NomeColaborador = resColaboradoresRelac.Resultado.First(x => x.Id == iGrupoPresencas.Key).Nome,
                DiasTrabalhados = new List<BsnRelacaoDoDiaTrabalhado>()
            };
            foreach (var iDiaRelacao in diasRelacao)
            {
                var iRelacaoDiaTrabalhado = new BsnRelacaoDoDiaTrabalhado { Dia = iDiaRelacao };
                var intervaloDia = iRelacaoDiaTrabalhado.ObterIntervaloDia();
                iRelacaoDiaTrabalhado.PresencasDoDia = iGrupoPresencas
                    .Where(x => intervaloDia.Inicio <= x.HoraMarcada && x.HoraMarcada < intervaloDia.Fim)
                    .Select(presencaDb => new BsnRelacaoDePresenca
                    {
                        Id = presencaDb.Id,
                        IdColaborador = presencaDb.IdColaborador,
                        NomeColaborador = iRelacaoHorasTrabalhadas.NomeColaborador,
                        IdEvento = presencaDb.IdEvento,
                        HoraMarcada = presencaDb.HoraMarcada
                    })
                    .ToList();
                iRelacaoDiaTrabalhado.CalcularMinutosTrabalhados(intervalosExpediente, intervalosStop);
                if (bsnPesquisa.MinutosTrabalhadosMinimo.HasValue && iRelacaoDiaTrabalhado.MinutosTrabalhados < bsnPesquisa.MinutosTrabalhadosMinimo.Value)
                {
                    continue;
                }
                if (bsnPesquisa.MinutosTrabalhadosMaximo.HasValue && iRelacaoDiaTrabalhado.MinutosTrabalhados > bsnPesquisa.MinutosTrabalhadosMaximo.Value)
                {
                    continue;
                }
                iRelacaoHorasTrabalhadas.DiasTrabalhados.Add(iRelacaoDiaTrabalhado);
            }
            if (iRelacaoHorasTrabalhadas.DiasTrabalhados.Any())
            {
                relacoesHorasTrabalhadas.Add(iRelacaoHorasTrabalhadas);
            }
        }
        return BsnResult<List<BsnRelacaoDeHorasDoColaborador>>.OkConteudo(relacoesHorasTrabalhadas);
    }
}