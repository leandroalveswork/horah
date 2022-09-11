namespace HoraH.Domain.Models.Bsn.Periodo;
public class BsnPeriodoPorDataArgs : IBsnPeriodoArgs
{
    public bool EPorMes => false;
    public DateTime DataInicioInclusive { get; set; }
    public DateTime DataFimInclusive { get; set; }
}