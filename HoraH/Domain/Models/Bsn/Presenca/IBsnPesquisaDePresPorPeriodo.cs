namespace HoraH.Domain.Models.Bsn.Presenca;
public interface IBsnPesquisaDePresPorPeriodo
{
    bool RelacaoObedeceOsFiltros(BsnRelacaoDeHorasTrabalhadas relacao);
}