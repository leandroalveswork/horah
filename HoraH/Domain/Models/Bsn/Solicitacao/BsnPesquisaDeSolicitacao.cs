using HoraH.Domain.Models.Bsn.Periodo;

namespace HoraH.Domain.Models.Bsn.Solicitacao;
public class BsnPesquisaDeSolicitacao
{
    public string? NomeSolicitador { get; set; }
    public string? IdTipoSolicitacao { get; set; }
    public IBsnPesquisaPorPeriodo Periodo { get; set; }
    public string? IdStatusSolicitacao { get; set; }
    public string? NomeAprovadorReprovador { get; set; }
    public static BsnPesquisaDeSolicitacao SemFiltros => new BsnPesquisaDeSolicitacao { Periodo = new BsnPesquisaPorPeriodoMes() };
}