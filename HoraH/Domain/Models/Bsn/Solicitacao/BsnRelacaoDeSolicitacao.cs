namespace HoraH.Domain.Models.Bsn.Solicitacao;
public class BsnRelacaoDeSolicitacao
{
    public string? Id { get; set; }
    public string? IdSolicitador { get; set; }
    public string? NomeSolicitador { get; set; }
    public string? IdTipoSolicitacao { get; set; }
    public DateTime HoraSolicitacao { get; set; }
    public string? IdStatusSolicitacao { get; set; }
    public string? IdAprovadorReprovador { get; set; }
    public string? NomeAprovadorReprovador { get; set; }
}