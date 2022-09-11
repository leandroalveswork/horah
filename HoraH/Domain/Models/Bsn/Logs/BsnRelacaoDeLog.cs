namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnRelacaoDeLog
{
    public string? Id { get; set; }
    public string? IdColaboradorOperador { get; set; }
    public string NomeColaboradorOperador { get; set; } = "";
    public string? IdOperacao { get; set; }
    public string? IdTabela { get; set; }
    public string IdEntidade { get; set; } = "";
    public List<BsnRelacaoDeDado> Dados { get; set; } = new List<BsnRelacaoDeDado>();
    public DateTime HoraOperacao { get; set; }
}
