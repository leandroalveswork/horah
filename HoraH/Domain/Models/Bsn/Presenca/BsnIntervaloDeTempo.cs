using HoraH.Domain.Models.Bsn.Evento;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnIntervaloDeTempo
{
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }

    public BsnIntervaloDeTempo? InterseccaoOrDefault(BsnIntervaloDeTempo outroIntervalo)
    {
        var maiorInicio = Inicio > outroIntervalo.Inicio ? Inicio : outroIntervalo.Inicio;
        var menorFim = Fim < outroIntervalo.Fim ? Fim : outroIntervalo.Fim;
        if (menorFim < maiorInicio)
        {
            return null;
        }
        return new BsnIntervaloDeTempo { Inicio = maiorInicio, Fim = menorFim };
    }

    public static List<BsnIntervaloDeTempo> ObterIntervalosExpediente(List<PresencaDbModel> presencasDb)
    {
        var intervalosRet = new List<BsnIntervaloDeTempo>();
        var idsPresencasContadas = new HashSet<string>();
        var fimExpedientes = presencasDb.Where(x => !BsnEventoLiterais.GetById(x.IdEvento).EInicioTrabalho && !BsnEventoLiterais.GetById(x.IdEvento).EEventoStop).OrderBy(x => x.HoraMarcada);
        foreach (var iPresencaDb in presencasDb.Where(x => BsnEventoLiterais.GetById(x.IdEvento).EInicioTrabalho && !BsnEventoLiterais.GetById(x.IdEvento).EEventoStop))
        {
            var fimExpediente = fimExpedientes.FirstOrDefault(x => x.HoraMarcada > iPresencaDb.HoraMarcada && x.HoraMarcada < iPresencaDb.HoraMarcada + TimeSpan.FromHours(12));
            if (fimExpediente == null)
            {
                break;
            }
            if (idsPresencasContadas.Contains(fimExpediente.Id))
            {
                continue;
            }
            var iIntervalo = new BsnIntervaloDeTempo { Inicio = iPresencaDb.HoraMarcada, Fim = fimExpediente.HoraMarcada };
            idsPresencasContadas.Add(fimExpediente.Id);
            intervalosRet.Add(iIntervalo);
        }
        return intervalosRet;
    }

    public static List<BsnIntervaloDeTempo> ObterIntervalosStop(List<PresencaDbModel> presencasDb)
    {
        var intervalosRet = new List<BsnIntervaloDeTempo>();
        var idsPresencasContadas = new HashSet<string>();
        var fimStops = presencasDb.Where(x => BsnEventoLiterais.GetById(x.IdEvento).EInicioTrabalho && BsnEventoLiterais.GetById(x.IdEvento).EEventoStop).OrderBy(x => x.HoraMarcada);
        foreach (var iPresencaDb in presencasDb.Where(x => !BsnEventoLiterais.GetById(x.IdEvento).EInicioTrabalho && BsnEventoLiterais.GetById(x.IdEvento).EEventoStop))
        {
            var fimStop = fimStops.FirstOrDefault(x => x.HoraMarcada > iPresencaDb.HoraMarcada && x.HoraMarcada < iPresencaDb.HoraMarcada + TimeSpan.FromHours(12));
            if (fimStop == null)
            {
                break;
            }
            if (idsPresencasContadas.Contains(fimStop.Id))
            {
                continue;
            }
            var iIntervalo = new BsnIntervaloDeTempo { Inicio = iPresencaDb.HoraMarcada, Fim = fimStop.HoraMarcada };
            idsPresencasContadas.Add(fimStop.Id);
            intervalosRet.Add(iIntervalo);
        }
        return intervalosRet;
    }
}