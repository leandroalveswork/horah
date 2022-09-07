using HoraH.Domain.Models.Bsn.Mes;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnPesquisaDePresencaPorMes : IBsnPesquisaDePresenca
{
    public string? NomeColaborador { get; set; }
    public string? IdEvento { get; set; }
    public int? MinutosTrabalhadosMinimo { get; set; }
    public int? MinutosTrabalhadosMaximo { get; set; }
    public bool FoiInformadoIntervaloMinutosTrabalhados => MinutosTrabalhadosMinimo.HasValue && MinutosTrabalhadosMaximo.HasValue;
    public string? IdMes { get; set; }
    public static BsnPesquisaDePresencaPorMes SemFiltro => new BsnPesquisaDePresencaPorMes();

    public bool DateEValido(DateTime dateVlr)
    {
        return string.IsNullOrEmpty(IdMes) || BsnMesLiterais.GetByHoraAsDateTime(dateVlr).Id == IdMes;
    }
}