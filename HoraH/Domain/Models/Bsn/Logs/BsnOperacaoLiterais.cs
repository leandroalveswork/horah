namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnOperacaoLiterais
{
    public static readonly BsnOperacao Inclusao = new BsnOperacao
    {
        Id = "1",
        Nome = "Inclusão"
    };
    public static readonly BsnOperacao Alteracao = new BsnOperacao
    {
        Id = "2",
        Nome = "Alteração"
    };
    public static readonly BsnOperacao Visualizacao = new BsnOperacao
    {
        Id = "3",
        Nome = "Visualização"
    };
    public static readonly BsnOperacao Exclusao = new BsnOperacao
    {
        Id = "4",
        Nome = "Exclusão"
    };
    public static List<BsnOperacao> ListarTodos()
    {
        return new List<BsnOperacao>
        {
            Inclusao, Alteracao, Exclusao
        };
    }
    
    public static BsnOperacao GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnOperacao? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}