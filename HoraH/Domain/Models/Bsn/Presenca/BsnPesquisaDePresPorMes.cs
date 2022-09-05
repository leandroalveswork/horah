using HoraH.Domain.Models.Bsn.Mes;

namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnPesquisaDePresPorMes : IBsnPesquisaDePresPorPeriodo
{
    public string? IdMes { get; set; }
    
    public bool RelacaoObedeceOsFiltros(BsnRelacaoDeHorasTrabalhadas relacao)
    {
        return BsnMesLiterais.GetByHoraAsDateTime(relacao.Dia).Id == IdMes;
    }
}