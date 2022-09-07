namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnRelacaoDeHorasDoColaborador
{
    public string IdColaborador { get; set; } = "";
    public string NomeColaborador { get; set; } = "";
    public List<BsnRelacaoDoDiaTrabalhado> DiasTrabalhados { get; set; } = new List<BsnRelacaoDoDiaTrabalhado>();
}