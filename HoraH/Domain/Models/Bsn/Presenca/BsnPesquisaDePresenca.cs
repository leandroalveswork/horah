namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnPesquisaDePresenca
{
    public string? NomeColaborador { get; set; }
    public string? IdEvento { get; set; }
    public int? MinutosTrabalhadosMinimo { get; set; } = null;
    public int? MinutosTrabalhadosMaximo { get; set; } = null;
    public IBsnPesquisaDePresPorPeriodo PesquisaPorPeriodo { get; set; } = new BsnPesquisaDePresPorMes();
    public static BsnPesquisaDePresenca SemFiltro => new BsnPesquisaDePresenca();
}