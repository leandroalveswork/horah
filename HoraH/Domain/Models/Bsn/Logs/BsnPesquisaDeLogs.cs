namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnPesquisaDeLogs
{
    public string? NomeColaborador { get; set; }
    public string? IdOperacao { get; set; }
    public string? IdTabela { get; set; }
    public string? IdEntidade { get; set; }
    public DateTime? DataOperacaoInicio { get; set; }
    public DateTime? DataOperacaoFim { get; set; }
    public BsnResult<object> ValidarRangeData()
    {
        if (DataOperacaoFim.HasValue && DataOperacaoInicio.HasValue && DataOperacaoFim.Value < DataOperacaoInicio.Value)
        {
            return BsnResult<object>.Erro("O intervalo de Data deve ser válido.");
        }
        return BsnResult<object>.Ok;
    }

    public static BsnPesquisaDeLogs SemFiltro => new BsnPesquisaDeLogs();
}