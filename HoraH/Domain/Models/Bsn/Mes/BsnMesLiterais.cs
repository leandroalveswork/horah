namespace HoraH.Domain.Models.Bsn.Mes;
public class BsnMesLiterais
{
    public static readonly BsnMes Jan = new BsnMes
    {
        Id = "1", Sigla = "Jan", Nome = "Janeiro"
    };
    public static readonly BsnMes Fev = new BsnMes
    {
        Id = "2", Sigla = "Fev", Nome = "Fevereiro"
    };
    public static readonly BsnMes Mar = new BsnMes
    {
        Id = "3", Sigla = "Mar", Nome = "Março"
    };
    public static readonly BsnMes Abr = new BsnMes
    {
        Id = "4", Sigla = "Abr", Nome = "Abril"
    };
    public static readonly BsnMes Mai = new BsnMes
    {
        Id = "5", Sigla = "Mai", Nome = "Maio"
    };
    public static readonly BsnMes Jun = new BsnMes
    {
        Id = "6", Sigla = "Jun", Nome = "Junho"
    };
    public static readonly BsnMes Jul = new BsnMes
    {
        Id = "7", Sigla = "Jul", Nome = "Julho"
    };
    public static readonly BsnMes Ago = new BsnMes
    {
        Id = "8", Sigla = "Ago", Nome = "Agosto"
    };
    public static readonly BsnMes Set = new BsnMes
    {
        Id = "9", Sigla = "Set", Nome = "Setembro"
    };
    public static readonly BsnMes Out = new BsnMes
    {
        Id = "10", Sigla = "Out", Nome = "Outubto"
    };
    public static readonly BsnMes Nov = new BsnMes
    {
        Id = "11", Sigla = "Nov", Nome = "Novembro"
    };
    public static readonly BsnMes Dez = new BsnMes
    {
        Id = "12", Sigla = "Dez", Nome = "Dezembro"
    };
    public static List<BsnMes> ListarTodos()
    {
        return new List<BsnMes> { Jan, Fev, Mar, Abr, Mai, Jun, Jul, Ago, Set, Out, Nov, Dez };
    }
    
    public static BsnMes GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnMes? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
    
    public static BsnMes GetBySigla(string sigla)
    {
        return ListarTodos().First(x => x.Sigla == sigla);
    }
    
    public static BsnMes? GetBySiglaOrDefault(string sigla)
    {
        return ListarTodos().FirstOrDefault(x => x.Sigla == sigla);
    }

    public static BsnMes GetByHoraAsDateTime(DateTime hora)
    {
        return GetById(hora.Month.ToString());
    }
}