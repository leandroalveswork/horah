namespace HoraH.Domain.Models.Bsn.Presenca.TipoInterseccao;
public class BsnTipoInterseccao
{
    public string Id { get; set; } = "";
    public string Nome { get; set; } = "";
    public Func<BsnIntervaloDeTempo, BsnIntervaloDeTempo, BsnIntervaloDeTempo?> ObterInterseccao;
}