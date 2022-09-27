using HoraH.Domain.Models.Bsn.Mes;

namespace HoraH.Domain.Models.Bsn.Periodo;
public class BsnPesquisaPorPeriodoMes : IBsnPesquisaPorPeriodo
{
    public string? IdMesInicioInclusive { get; set; }
    public string? IdAnoInicioInclusive { get; set; }
    public string? IdMesFimInclusive { get; set; }
    public string? IdAnoFimInclusive { get; set; }
    private DateTime? ObterDataInicio()
    {
        if (string.IsNullOrEmpty(IdMesInicioInclusive) || !int.TryParse(IdAnoInicioInclusive, out int anoInicioAsInt))
        {
            return null;
        }
        var dtInicio = new DateTime(anoInicioAsInt, BsnMesLiterais.GetById(IdMesInicioInclusive).ObterMonthComecandoUm(), 1);
        return dtInicio;
    }
    private DateTime? ObterDataFim()
    {
        if (string.IsNullOrEmpty(IdMesFimInclusive) || !int.TryParse(IdAnoFimInclusive, out int anoFimAsInt))
        {
            return null;
        }
        var dtFim = new DateTime(anoFimAsInt, BsnMesLiterais.GetById(IdMesFimInclusive).ObterMonthComecandoUm(), 1);
        dtFim = dtFim.AddMonths(1);
        return dtFim;
    }

    public bool DateEValido(DateTime dateVlr)
    {
        if (!string.IsNullOrEmpty(IdMesInicioInclusive) && ObterDataInicio() > dateVlr)
        {
            return false;
        }
        return string.IsNullOrEmpty(IdMesFimInclusive) || dateVlr < ObterDataFim();
    }

    public BsnResult<object> ValidarRangesEObrigatorios()
    {
        if ((string.IsNullOrEmpty(IdMesInicioInclusive) != string.IsNullOrEmpty(IdAnoInicioInclusive)) ||
            (string.IsNullOrEmpty(IdMesFimInclusive) != string.IsNullOrEmpty(IdAnoFimInclusive)))
        {
            return BsnResult<object>.Erro("O Mês e Ano devem ou ser preenchidos em conjunto, ou ambos deixados em branco.");
        }
        var dtInicio = ObterDataInicio();
        var dtFim = ObterDataFim();
        if (dtFim.HasValue && dtInicio.HasValue && dtFim.Value < dtInicio.Value)
        {
            return BsnResult<object>.Erro("O intervalo de Mês/Ano deve ser válido.");
        }
        return BsnResult<object>.Ok;
    }
}