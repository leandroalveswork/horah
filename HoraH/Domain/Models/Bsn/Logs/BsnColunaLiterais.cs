using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnColunaLiterais
{
    public static List<BsnColuna> ObterColunasDaDbModel(string nomeColecao, Type dbModelType)
    {
        var props = dbModelType.GetProperties();
        var colunas = props.Select(propt => new BsnColuna
        {
            Id = (nomeColecao + "_" + propt.Name).GetHashCode().ToString(),
            Nome = nomeColecao + "_" + propt.Name,
            NomeTabela = nomeColecao,
            NomeColuna = propt.Name
        }).ToList();
        return colunas;
    }

    public static BsnColuna ObterColunaEspecifica(string nomeColecao, string nomePropt)
    {
        var coluna = new BsnColuna
        {
            Id = (nomeColecao + "_" + nomePropt).GetHashCode().ToString(),
            Nome = nomeColecao + "_" + nomePropt,
            NomeTabela = nomeColecao,
            NomeColuna = nomePropt
        };
        return coluna;
    }

    public static List<BsnColuna> ListarTodos()
    {
        var lRange = new List<BsnColuna>();
        lRange.AddRange(ObterColunasDaDbModel("Colaborador", typeof(ColaboradorDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("Acesso", typeof(AcessoDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("Presenca", typeof(PresencaDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("Dado", typeof(DadoDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("Registro", typeof(RegistroDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("AlteracaoRegistro", typeof(AlteracaoRegistroDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("VisualizacaoRegistro", typeof(VisualizacaoRegistroDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("Solicitacao", typeof(SolicitacaoDbModel)));
        lRange.AddRange(ObterColunasDaDbModel("ItemSolicitacao", typeof(ItemSolicitacaoDbModel)));
        return lRange;
    }
    
    public static BsnColuna GetById(string id)
    {
        return ListarTodos().First(x => x.Id == id);
    }
    
    public static BsnColuna? GetByIdOrDefault(string id)
    {
        return ListarTodos().FirstOrDefault(x => x.Id == id);
    }
}