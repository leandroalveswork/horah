namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnTipoRegistroLiterais
{
    public static readonly BsnTipoRegistro Inclusao = new BsnTipoRegistro
    {
        Id = "1",
        Nome = "Inclusão"
    };
    public static readonly BsnTipoRegistro Alteracao = new BsnTipoRegistro
    {
        Id = "2",
        Nome = "Alteração"
    };
    public static readonly BsnTipoRegistro Visualizacao = new BsnTipoRegistro
    {
        Id = "3",
        Nome = "Visualização"  
    };
    public static List<BsnTipoRegistro> ListarTodos()
    {
        return new List<BsnTipoRegistro>
        {
            Inclusao, Alteracao, Visualizacao
        };
    }
    
    public static BsnTipoRegistro GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnTipoRegistro? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}