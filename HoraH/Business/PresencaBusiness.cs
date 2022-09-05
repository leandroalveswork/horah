using HoraH.Domain.Design.Patterns.Strategy;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Evento;
using HoraH.Domain.Models.Bsn.Presenca;
using HoraH.Domain.Models.DbModels;

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
    public async Task<BsnResult<List<BsnRelacaoDeHorasTrabalhadas>>> PesquisarAsync(BsnPesquisaDePresenca bsnPesquisa)
    {
        var presencas = await _presencaRepository.SelectAllAsync();
        var resColaboradoresComONome = await _colaboradorBusiness.PesquisarAsync(new BsnPesquisaDeColaborador { Nome = bsnPesquisa.NomeColaborador, EstaAtivo = true });
        if (!resColaboradoresComONome.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeHorasTrabalhadas>>.Erro(resColaboradoresComONome.Mensagem);
        }
        var idsColaboradoresComONome = new HashSet<string>(resColaboradoresComONome.Resultado.Select(x => x.Id));
        var relacoesDePresencas = presencas.Where(x => idsColaboradoresComONome.Contains(x.IdColaborador));
        if (!string.IsNullOrEmpty(bsnPesquisa.IdEvento))
        {
            relacoesDePresencas = relacoesDePresencas.Where(x => x.IdEvento == bsnPesquisa.IdEvento);
        }
        var relacoesDeHorasTrabalhadas = relacoesDePresencas.GroupBy(x => x.HoraMarcada.ToString("yyyyMMdd") + "|" + x.IdColaborador)
            .Select(relacoesDePresencasGrp => new BsnRelacaoDeHorasTrabalhadas
            {
                IdColaborador = relacoesDePresencasGrp.First().IdColaborador,
                NomeColaborador = resColaboradoresComONome.Resultado.First(x => relacoesDePresencasGrp.First().IdColaborador == x.Id).Nome,
                MinutosTrabalhados = ObterMinutosTrabalhados(relacoesDePresencasGrp),
                Dia = relacoesDePresencasGrp.First().HoraMarcada.Date,
                PresencasNoDia = relacoesDePresencasGrp.Select(relacDePresenca => new BsnRelacaoDePresenca
                {
                    Id = relacDePresenca.Id,
                    IdColaborador = relacDePresenca.IdColaborador,
                    NomeColaborador = resColaboradoresComONome.Resultado.First(x => relacDePresenca.IdColaborador == x.Id).Nome,
                    IdEvento = relacDePresenca.IdEvento,
                    HoraMarcada = relacDePresenca.HoraMarcada
                }).ToList()
            });
        relacoesDeHorasTrabalhadas = HrhFiltradorAnulavel.FiltrarPeloPredicate(relacoesDeHorasTrabalhadas, x => x.MinutosTrabalhados >= bsnPesquisa.MinutosTrabalhadosMinimo, bsnPesquisa.MinutosTrabalhadosMinimo);
        relacoesDeHorasTrabalhadas = HrhFiltradorAnulavel.FiltrarPeloPredicate(relacoesDeHorasTrabalhadas, x => x.MinutosTrabalhados >= bsnPesquisa.MinutosTrabalhadosMaximo, bsnPesquisa.MinutosTrabalhadosMaximo);
        relacoesDeHorasTrabalhadas = relacoesDeHorasTrabalhadas.Where(x => bsnPesquisa.PesquisaPorPeriodo.RelacaoObedeceOsFiltros(x));
        return BsnResult<List<BsnRelacaoDeHorasTrabalhadas>>.OkConteudo(relacoesDeHorasTrabalhadas.ToList());
    }

    private int ObterMinutosTrabalhados(IGrouping<string, PresencaDbModel> relacaoHoras)
    {
        var totalMinutos = 0;
        foreach (var iInicioExpediente in relacaoHoras.Where(x => x.IdEvento == BsnEventoLiterais.InicioExpediente.Id))
        {
            var fimExpediente = relacaoHoras.FirstOrDefault(x => x.IdEvento == BsnEventoLiterais.FimExpediente.Id && x.HoraMarcada > iInicioExpediente.HoraMarcada);
            if (fimExpediente == null)
            {
                break;
            }
            var tempoTrabalhado = fimExpediente.HoraMarcada - iInicioExpediente.HoraMarcada;
            var intervalos = ObterIntervalosQueInterceptam(relacaoHoras, iInicioExpediente.HoraMarcada, fimExpediente.HoraMarcada);
            foreach (var iIntervalo in intervalos)
            {
                tempoTrabalhado -= iIntervalo.ObterTempoEmComum(iInicioExpediente.HoraMarcada, fimExpediente.HoraMarcada);
            }
            totalMinutos += Convert.ToInt32(Math.Floor(tempoTrabalhado.TotalMinutes));
        }
        return totalMinutos;
    }

    private IEnumerable<BsnIntervaloDeTempo> ObterIntervalosQueInterceptam(IEnumerable<PresencaDbModel> presencas, DateTime tInicio, DateTime tFim)
    {
        var idsFimIntervalo = new HashSet<string>();
        var intervalosTotal = new List<BsnIntervaloDeTempo>();
        foreach (var iInicioIntervalo in presencas)
        {
            var eInicioIntervalo = BsnEventoLiterais.GetById(iInicioIntervalo.IdEvento).EEventoDeIntervalo && !BsnEventoLiterais.GetById(iInicioIntervalo.IdEvento).EInicioTrabalho;
            if (!eInicioIntervalo)
            {
                continue;
            }
            var fimIntervalo = presencas.FirstOrDefault(x => BsnEventoLiterais.GetById(iInicioIntervalo.IdEvento).EEventoDeIntervalo && BsnEventoLiterais.GetById(iInicioIntervalo.IdEvento).EInicioTrabalho && x.HoraMarcada > iInicioIntervalo.HoraMarcada);
            if (fimIntervalo == null || idsFimIntervalo.Contains(fimIntervalo.Id)) 
            {
                continue;
            }
            var novoIntervaloAdd = new BsnIntervaloDeTempo { Inicio = iInicioIntervalo.HoraMarcada, Fim = fimIntervalo.HoraMarcada };
            if (novoIntervaloAdd.ObterTempoEmComum(tInicio, tFim) > TimeSpan.Zero)
            {
                idsFimIntervalo.Add(fimIntervalo.Id);
                intervalosTotal.Add(novoIntervaloAdd);
            }
        }
        return intervalosTotal;
    }
}