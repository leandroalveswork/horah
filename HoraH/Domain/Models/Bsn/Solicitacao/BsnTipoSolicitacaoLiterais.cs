namespace HoraH.Domain.Models.Bsn.Solicitacao;
public class BsnTipoSolicitacaoLiterais
{
    public static readonly BsnTipoSolicitacao LiberarAcesso = new BsnTipoSolicitacao
    {
        Id = "1",
        Nome = "Liberar Acesso"
    };
    public static readonly BsnTipoSolicitacao IncluirPresencaFaltante = new BsnTipoSolicitacao
    {
        Id = "2",
        Nome = "Incluir Presença Faltante"
    };
    public static readonly BsnTipoSolicitacao AlterarPresencaErrada = new BsnTipoSolicitacao
    {
        Id = "3",
        Nome = "Alterar Presença Errada"
    };
    public static readonly BsnTipoSolicitacao ExcluirPresencaErrada = new BsnTipoSolicitacao
    {
        Id = "4",
        Nome = "Excluir Presença Errada"
    };
    public static List<BsnTipoSolicitacao> ListarTodos()
    {
        return new List<BsnTipoSolicitacao>
        {
            LiberarAcesso, IncluirPresencaFaltante, AlterarPresencaErrada, ExcluirPresencaErrada
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