namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnRelacaoDeLog
{
    public string? IdColaborador { get; set; }
    public string NomeColaborador { get; set; } = "";
    public string? IdOperacao { get; set; }
    public string? IdTabela { get; set; }
    public DateTime HoraOperacao { get; set; }
    public List<BsnRelacaoDeDado> Colunas { get; set; } = new List<BsnRelacaoDeDado>();
}
