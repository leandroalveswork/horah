namespace HoraH.Domain.Models.Bsn.Solicitacao;
public class BsnStatusSolicitacaoLiterais
{
    public static readonly BsnStatusSolicitacao Aguardando = new BsnStatusSolicitacao
    {
        Id = "1",
        Nome = "Aguardando",
        FoiAprovadoOuReprovado = false
    };
    public static readonly BsnStatusSolicitacao Reprovada = new BsnStatusSolicitacao
    {
        Id = "2",
        Nome = "Reprovada",
        FoiAprovadoOuReprovado = true
    };
    public static readonly BsnStatusSolicitacao Aprovada = new BsnStatusSolicitacao
    {
        Id = "3",
        Nome = "Aprovada",
        FoiAprovadoOuReprovado = true
    };
    public static List<BsnStatusSolicitacao> ListarTodos()
    {
        return new List<BsnStatusSolicitacao>
        {
            Aguardando, Reprovada, Aprovada
        };
    }
    
    public static BsnStatusSolicitacao GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnStatusSolicitacao? GetByIdOrDefault(string id)
    {
        if (id is null)
        {
            return null;
        }
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}