namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnRelacaoDeHorasTrabalhadas
{
    public string IdColaborador { get; set; } = "";
    public string NomeColaborador { get; set; } = "";
    public int MinutosTrabalhados { get; set; } = 0;
    public DateTime Dia { get; set; }
    public List<BsnRelacaoDePresenca> PresencasNoDia { get; set; } = new List<BsnRelacaoDePresenca>();
}