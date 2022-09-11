namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnPesquisaDeLogs
{
    public string? NomeColaborador { get; set; }
    public string? IdOperacao { get; set; }
    public string? IdTabela { get; set; }
    public DateTime? HoraOperacaoInicio { get; set; }
    public DateTime? HoraOperacaoFim { get; set; }
    public BsnResult<object> ValidarRangeHoras()
    {
        if (HoraOperacaoFim.HasValue && HoraOperacaoInicio.HasValue && HoraOperacaoFim.Value < HoraOperacaoInicio.Value)
        {
            return BsnResult<object>.Erro("O intervalo de Horas deve ser válido.");
        }
        return BsnResult<object>.Ok;
    }
}