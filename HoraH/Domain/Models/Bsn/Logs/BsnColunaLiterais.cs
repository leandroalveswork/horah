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
            NomeTabela = nomeColecao
        }).ToList();
        return colunas;
    }

    public static BsnColuna ObterColunaEspecifica(string nomeColecao, string nomePropt)
    {
        var coluna = new BsnColuna
        {
            Id = (nomeColecao + "_" + nomePropt).GetHashCode().ToString(),
            Nome = nomeColecao + "_" + nomePropt,
            NomeTabela = nomeColecao
        };
        return coluna;
    }
}