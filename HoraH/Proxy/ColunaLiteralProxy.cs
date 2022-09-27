using HoraH.Domain.Interfaces.Proxy;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn.Logs;

namespace HoraH.Proxy;
public class ColunaLiteralProxy : IColunaLiteralProxy
{
    public BsnColuna ObterColunaEspecificaPorNome(string nomeColecao, string nomeColuna, Type dbModelType)
    {
        var coluna = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, nomeColuna, dbModelType);
        return coluna.DuplicarNaMemoria();
    }
}