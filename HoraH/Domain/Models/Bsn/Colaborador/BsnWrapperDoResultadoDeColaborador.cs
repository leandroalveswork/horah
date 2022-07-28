namespace HoraH.Domain.Models.Bsn.Colaborador;
public class BsnWrapperDoResultadoDeColaborador
{
    public List<BsnResultadoDeColaborador> Resultados { get; set; } = new List<BsnResultadoDeColaborador>();
    public int Total { get; set; }
    public int NumeroDaPaginaCorrigido { get; set; }
}