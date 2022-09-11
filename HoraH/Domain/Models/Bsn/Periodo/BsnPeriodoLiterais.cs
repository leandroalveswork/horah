using HoraH.Domain.Models.Bsn.Mes;

namespace HoraH.Domain.Models.Bsn.Periodo;
public class BsnPeriodoLiterais
{
    public static readonly BsnPeriodo EstaSemana = new BsnPeriodo
    {
        Id = "1",
        Nome = "Esta Semana",
        PeriodoArgs = new BsnPeriodoPorDataArgs {
            DataInicioInclusive = DateTime.Today.AddDays(-((int)DateTime.Now.DayOfWeek)),
            DataFimInclusive = DateTime.Today.AddDays(-((int)DateTime.Now.DayOfWeek) + 6)
        }
    };
    public static readonly BsnPeriodo UltimaSemana = new BsnPeriodo
    {
        Id = "2",
        Nome = "Última Semana",
        PeriodoArgs = new BsnPeriodoPorDataArgs {
            DataInicioInclusive = DateTime.Today.AddDays(-((int)DateTime.Now.DayOfWeek) - 7),
            DataFimInclusive = DateTime.Today.AddDays(-((int)DateTime.Now.DayOfWeek) - 7 + 6)
        }
    };
    public static readonly BsnPeriodo Ultimos7Dias = new BsnPeriodo
    {
        Id = "3",
        Nome = "Últimos 7 dias",
        PeriodoArgs = new BsnPeriodoPorDataArgs {
            DataInicioInclusive = DateTime.Today.AddDays(-7),
            DataFimInclusive = DateTime.Today.AddDays(-1)
        }
    };
    public static readonly BsnPeriodo EsteMes = new BsnPeriodo
    {
        Id = "4",
        Nome = "Este Mês",
        PeriodoArgs = new BsnPeriodoPorMesArgs {
            IdMes = BsnMesLiterais.GetByHoraAsDateTime(DateTime.Today).Id,
            IdAno = DateTime.Today.Year.ToString(),
        }
    };
    public static readonly BsnPeriodo UltimoMes = new BsnPeriodo
    {
        Id = "5",
        Nome = "Último Mês",
        PeriodoArgs = new BsnPeriodoPorMesArgs {
            IdMes = BsnMesLiterais.GetByHoraAsDateTime(DateTime.Today.AddMonths(-1)).Id,
            IdAno = DateTime.Today.Year.ToString(),
        }
    };
    public static readonly BsnPeriodo Ultimos30Dias = new BsnPeriodo
    {
        Id = "6",
        Nome = "Últimos 30 dias",
        PeriodoArgs = new BsnPeriodoPorDataArgs {
            DataInicioInclusive = DateTime.Today.AddDays(-30),
            DataFimInclusive = DateTime.Today.AddDays(-1)
        }
    };
    public static List<BsnPeriodo> ListarTodos()
    {
        return new List<BsnPeriodo>
        {
            EstaSemana, UltimaSemana, Ultimos7Dias, EsteMes, UltimoMes, Ultimos30Dias
        };
    }
    
    public static BsnPeriodo GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnPeriodo? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}