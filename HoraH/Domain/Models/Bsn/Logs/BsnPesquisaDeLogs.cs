namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnPesquisaDeLogs
{
    public string? NomeColaborador { get; set; }
    public string? IdOperacao { get; set; }
    public string? IdTabela { get; set; }
    public DateTime? HoraOperacaoInicio { get; set; }
    public DateTime? HoraOperacaoFim { get; set; }
}