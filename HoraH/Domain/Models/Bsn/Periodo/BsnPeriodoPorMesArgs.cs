namespace HoraH.Domain.Models.Bsn.Periodo;
public class BsnPeriodoPorMesArgs : IBsnPeriodoArgs
{
    public bool EPorMes => true;
    public string IdMes { get; set; } = "";
    public string IdAno { get; set; } = "";
}