using HoraH.Domain.Models.Bsn.Common;

namespace HoraH.Domain.Models.Bsn.Colaborador;
public class BsnPesquisaDeColaborador : BsnPesquisaBase
{
    public string? Nome { get; set; }
    public string? Login { get; set; }
    public bool? EstaAtivo { get; set; }
    public static BsnPesquisaDeColaborador SemFiltro => new BsnPesquisaDeColaborador();
}