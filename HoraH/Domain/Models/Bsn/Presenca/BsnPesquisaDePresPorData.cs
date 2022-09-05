namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnPesquisaDePresPorData : IBsnPesquisaDePresPorPeriodo
{
    public DateTime DataInicioInclusive { get; set; }
    public DateTime DataFimInclusive { get; set; }

    public bool RelacaoObedeceOsFiltros(BsnRelacaoDeHorasTrabalhadas relacao)
    {
        return (relacao.Dia >= DataInicioInclusive) && (relacao.Dia < (DataFimInclusive + TimeSpan.FromDays(1)));
    }
}