namespace HoraH.Domain.Models.Bsn.Colaborador;
public class BsnPesquisaDeColaborador
{
    public string? Nome { get; set; }
    public string? Login { get; set; }
    public bool? EstaAtivo { get; set; }
    public static BsnPesquisaDeColaborador SemFiltro => new BsnPesquisaDeColaborador();
}