using HoraH.Domain.Models.LinqExp;

namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnPesquisaDePresencaPorData : IBsnPesquisaDePresenca
{
    public string? NomeColaborador { get; set; }
    public string? IdEvento { get; set; }
    public int? MinutosTrabalhadosMinimo { get; set; }
    public int? MinutosTrabalhadosMaximo { get; set; }
    public bool FoiInformadoIntervaloMinutosTrabalhados => MinutosTrabalhadosMinimo.HasValue && MinutosTrabalhadosMaximo.HasValue;
    public DateTime? DataInicioInclusive { get; set; }
    public DateTime? DataFimInclusive { get; set; }
    public static BsnPesquisaDePresencaPorData SemFiltro => new BsnPesquisaDePresencaPorData();

    public bool DateEValido(DateTime dateVlr)
    {
        if (DataInicioInclusive.HasValue && dateVlr < DataInicioInclusive.Value)
        {
            return false;
        }
        return !(DataFimInclusive.HasValue && dateVlr > DataFimInclusive.Value);
    }

    public BsnResult<object> ValidarRanges()
    {
        if (MinutosTrabalhadosMaximo.HasValue && MinutosTrabalhadosMinimo.HasValue && MinutosTrabalhadosMaximo.Value < MinutosTrabalhadosMinimo.Value)
        {
            return BsnResult<object>.Erro("O intervalo de Minutos deve ser válido.");
        }
        if (DataFimInclusive.HasValue && DataInicioInclusive.HasValue && DataFimInclusive.Value < DataInicioInclusive.Value)
        {
            return BsnResult<object>.Erro("O intervalo de Data Específica deve ser válido.");
        }
        return BsnResult<object>.Ok;
    }
}