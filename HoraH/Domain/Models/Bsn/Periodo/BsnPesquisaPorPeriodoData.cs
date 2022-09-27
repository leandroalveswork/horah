namespace HoraH.Domain.Models.Bsn.Periodo;
public class BsnPesquisaPorPeriodoData : IBsnPesquisaPorPeriodo
{
    public DateTime? DataInicioInclusive { get; set; }
    public DateTime? DataFimInclusive { get; set; }

    public bool DateEValido(DateTime dateVlr)
    {
        if (DataInicioInclusive.HasValue && dateVlr < DataInicioInclusive.Value)
        {
            return false;
        }
        return !(DataFimInclusive.HasValue && dateVlr > DataFimInclusive.Value);
    }

    public BsnResult<object> ValidarRangesEObrigatorios()
    {
        if (DataFimInclusive.HasValue && DataInicioInclusive.HasValue && DataFimInclusive.Value < DataInicioInclusive.Value)
        {
            return BsnResult<object>.Erro("O intervalo de Data Específica deve ser válido.");
        }
        return BsnResult<object>.Ok;
    }
}