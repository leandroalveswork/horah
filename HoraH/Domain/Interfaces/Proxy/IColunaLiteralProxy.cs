using HoraH.Domain.Models.Bsn.Logs;

namespace HoraH.Domain.Interfaces.Proxy;
public interface IColunaLiteralProxy
{
    BsnColuna ObterColunaEspecificaPorNome(string nomeColecao, string nomeColuna, Type dbModelType);
}