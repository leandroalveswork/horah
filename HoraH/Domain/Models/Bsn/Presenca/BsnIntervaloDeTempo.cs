namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnIntervaloDeTempo
{
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
    public TimeSpan ObterTempoEmComum(DateTime tInicio, DateTime tFim)
    {
        if (Inicio > tFim || Fim < tInicio)
        {
            return TimeSpan.Zero;
        }
        var maiorInicio = Inicio > tInicio ? Inicio : tInicio;
        var menorFim = Fim < tFim ? Fim : tFim;
        return menorFim - maiorInicio;
    }
}