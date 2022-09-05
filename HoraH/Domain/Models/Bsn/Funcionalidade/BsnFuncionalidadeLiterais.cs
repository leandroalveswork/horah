namespace HoraH.Domain.Models.Bsn.Funcionalidade;
public class BsnFuncionalidadeLiterais
{
    public static readonly BsnFuncionalidade ListarColaborador = new BsnFuncionalidade
    {
        Id = "1", Nome = "Listar Colaborador"
    };
    public static readonly BsnFuncionalidade ListarAcesso = new BsnFuncionalidade
    {
        Id = "2", Nome = "Listar Acesso"
    };
    public static readonly BsnFuncionalidade AlterarAcesso = new BsnFuncionalidade
    {
        Id = "3", Nome = "Permitir/Bloquear Acesso"
    };
    public static readonly BsnFuncionalidade AtivarColaborador = new BsnFuncionalidade
    {
        Id = "4", Nome = "Ativar/Inativar Colaborador"
    };
    public static readonly BsnFuncionalidade ListarLog = new BsnFuncionalidade
    {
        Id = "5", Nome = "Listar Log"
    };
    public static readonly BsnFuncionalidade MarcarPresenca = new BsnFuncionalidade
    {
        Id = "6", Nome = "Marcar Presença"
    };
    public static readonly BsnFuncionalidade ListarPresenca = new BsnFuncionalidade
    {
        Id = "7", Nome = "Listar Presença"
    };
    public static readonly BsnFuncionalidade IncluirSolicitacao = new BsnFuncionalidade
    {
        Id = "8", Nome = "Solicitar"
    };
    public static readonly BsnFuncionalidade ListarSolicitacao = new BsnFuncionalidade
    {
        Id = "9", Nome = "Listar Solicitação"
    };
    public static readonly BsnFuncionalidade AprovarSolicitacao = new BsnFuncionalidade
    {
        Id = "10", Nome = "Aprovar/Reprovar Solicitação"
    };
    public static List<BsnFuncionalidade> ListarTodos()
    {
        return new List<BsnFuncionalidade>
        {
            ListarColaborador,
            ListarAcesso,
            AlterarAcesso,
            AtivarColaborador,
            ListarLog,
            MarcarPresenca,
            ListarPresenca,
            IncluirSolicitacao,
            ListarSolicitacao,
            AprovarSolicitacao
        };
    }
    
    public static BsnFuncionalidade GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnFuncionalidade? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}