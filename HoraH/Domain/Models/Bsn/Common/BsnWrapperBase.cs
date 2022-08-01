namespace HoraH.Domain.Models.Bsn.Common;
public class BsnWrapperBase<TResultado>
{
    public List<TResultado> Resultados { get; set; } = new List<TResultado>();
    public int Total { get; set; }
    public int NumeroDaPaginaCorrigido { get; set; }
}