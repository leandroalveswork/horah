namespace HoraH.Domain.Models.Bsn.Common;
public class BsnPesquisaBase
{
    public int NumeroDaColuna { get; set; } = 0;
    public bool ECrescente { get; set; } = true;
    public int NumeroDaPagina { get; set; } = 1;
    public int ResultadosPorPagina { get; set; } = 5;
}