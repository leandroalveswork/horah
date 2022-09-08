namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnTabelaLiterais
{
    public static readonly BsnTabela Colaborador = new BsnTabela
    {
        Id = "1",
        Nome = "Colaborador"
    };
    public static readonly BsnTabela Acesso = new BsnTabela
    {
        Id = "2",
        Nome = "Acesso"
    };
    public static readonly BsnTabela Presenca = new BsnTabela
    {
        Id = "3",
        Nome = "Presenca"
    };
    public static readonly BsnTabela Dado = new BsnTabela
    {
        Id = "4",
        Nome = "Dado"
    };
    public static readonly BsnTabela Registro = new BsnTabela
    {
        Id = "5",
        Nome = "Registro"
    };
    public static readonly BsnTabela AlteracaoRegistro = new BsnTabela
    {
        Id = "6",
        Nome = "AlteracaoRegistro"
    };
    public static readonly BsnTabela Solicitacao = new BsnTabela
    {
        Id = "7",
        Nome = "Solicitacao"
    };
    public static List<BsnTabela> ListarTodos()
    {
        return new List<BsnTabela>
        {
            Colaborador, Acesso, Presenca, Dado, Registro, AlteracaoRegistro, Solicitacao
        };
    }
    
    public static BsnTabela GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnTabela? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
    
    public static BsnTabela GetByNome(string nome)
    {
        return ListarTodos().First(x => x.Nome == nome);
    }
    
    public static BsnTabela? GetByNomeOrDefault(string nome)
    {
        return ListarTodos().FirstOrDefault(x => x.Nome == nome);
    }
}