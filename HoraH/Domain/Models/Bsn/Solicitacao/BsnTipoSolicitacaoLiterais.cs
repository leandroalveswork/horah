namespace HoraH.Domain.Models.Bsn.Solicitacao;
public class BsnTipoSolicitacaoLiterais
{
    public static readonly BsnTipoSolicitacao LiberarAcesso = new BsnTipoSolicitacao
    {
        Id = "1",
        Nome = "Liberar Acesso"
    };
    public static readonly BsnTipoSolicitacao BloquearAcesso = new BsnTipoSolicitacao
    {
        Id = "2",
        Nome = "Bloquear Acesso"
    };
    public static readonly BsnTipoSolicitacao IncluirPresenca = new BsnTipoSolicitacao
    {
        Id = "3",
        Nome = "Incluir Presença"
    };
    public static readonly BsnTipoSolicitacao AlterarPresenca = new BsnTipoSolicitacao
    {
        Id = "4",
        Nome = "Alterar Presença"
    };
    public static readonly BsnTipoSolicitacao ExcluirPresenca = new BsnTipoSolicitacao
    {
        Id = "5",
        Nome = "Excluir Presença"
    };
    public static List<BsnTipoSolicitacao> ListarTodos()
    {
        return new List<BsnTipoSolicitacao>
        {
            LiberarAcesso, BloquearAcesso, IncluirPresenca, AlterarPresenca, ExcluirPresenca
        };
    }
    
    public static BsnTipoSolicitacao GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnTipoSolicitacao? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}