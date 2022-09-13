using System.Reflection;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnColunaLiterais
{
    public static List<BsnColuna> ObterColunasDaDbModel(string nomeColecao, Type dbModelType)
    {
        var props = dbModelType.GetProperties();
        var colunas = props.Select(propt => ObterColunaEspecifica(nomeColecao, propt)).ToList();
        return colunas;
    }

    public static BsnColuna ObterColunaEspecifica(string nomeColecao, PropertyInfo proptInfo)
    {
        var coluna = new BsnColuna
        {
            Id = (nomeColecao + "_" + proptInfo.Name).GetHashCode().ToString(),
            Nome = nomeColecao + "_" + proptInfo.Name,
            NomeTabela = nomeColecao,
            NomeColuna = proptInfo.Name,
            TipoOriginal = proptInfo.PropertyType
        };
        return coluna;
    }

    public static BsnColuna ObterColunaEspecificaPorNome(string nomeColecao, string nomeColuna, Type dbModelType)
    {
        var props = dbModelType.GetProperties();
        var propEncontrada = props.FirstOrDefault(x => x.Name == nomeColuna);
        if (propEncontrada == null)
        {
            throw new ArgumentOutOfRangeException("Type " + dbModelType.Name + " não tem a propriedade " + nomeColuna);
        }
        return ObterColunaEspecifica(nomeColecao, propEncontrada);
    }

    public static List<string> ListarIdsColunasSingle(string nomeColecao, string nomeColuna, Type dbModelType)
    {
        return new List<string> { ObterColunaEspecificaPorNome(nomeColecao, nomeColuna, dbModelType).Id };
    }

    public static List<string> ListarIdsColunas(string nomeColecao, IEnumerable<string> nomesColunas, Type dbModelType)
    {
        return nomesColunas.Select(x => ObterColunaEspecificaPorNome(nomeColecao, x, dbModelType).Id).ToList();
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

    public static List<string> ObterColunasSaoId()
    {
        var idsColunasSaoId = BsnColunaLiterais.ListarTodos().Where(x => x.NomeColuna == "Id").Select(x => x.Id);
        return idsColunasSaoId.ToList();
    }
}