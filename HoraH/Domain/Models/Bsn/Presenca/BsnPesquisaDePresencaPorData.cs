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
}