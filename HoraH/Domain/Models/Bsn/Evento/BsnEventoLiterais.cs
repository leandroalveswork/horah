namespace HoraH.Domain.Models.Bsn.Evento;
public class BsnEventoLiterais
{
    public static readonly BsnEvento InicioExpediente = new BsnEvento
    {
        Id = "1", Nome = "Início Expediente", EInicioTrabalho = true, EEventoStop = false
    };
    public static readonly BsnEvento FimExpediente = new BsnEvento
    {
        Id = "2", Nome = "Fim Expediente", EInicioTrabalho = false, EEventoStop = false
    };
    public static readonly BsnEvento SaidaAlmoco = new BsnEvento
    {
        Id = "3", Nome = "Saída para Almoço", EInicioTrabalho = false, EEventoStop = true
    };
    public static readonly BsnEvento RetornoAlmoco = new BsnEvento
    {
        Id = "4", Nome = "Retorno do Almoço", EInicioTrabalho = true, EEventoStop = true
    };
    public static readonly BsnEvento InicioAusencia = new BsnEvento
    {
        Id = "5", Nome = "Início Ausência", EInicioTrabalho = false, EEventoStop = true
    };
    public static readonly BsnEvento FimAusencia = new BsnEvento
    {
        Id = "6", Nome = "Fim Ausência", EInicioTrabalho = true, EEventoStop = true
    };
    public static List<BsnEvento> ListarTodos()
    {
        return new List<BsnEvento>
        {
            InicioExpediente,
            FimExpediente,
            SaidaAlmoco,
            RetornoAlmoco,
            InicioAusencia,
            FimAusencia
        };
    }
    
    public static BsnEvento GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnEvento? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}