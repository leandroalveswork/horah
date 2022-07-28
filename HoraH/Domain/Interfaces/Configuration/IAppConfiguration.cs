using HoraH.Domain.Models.Settings;

namespace HoraH.Domain.Interfaces.Configuration;
public interface IAppConfiguration
{
    string ConexaoBD { get; }
    string NomeBD { get; }
    string NomeColecColaborador { get; }
    string NomeColecAcesso { get; }
    string NomeColecFuncionalidade { get; }
    string NomeColecPresenca { get; }
    string NomeColecSolicitacaoAcesso { get; }
    string NomeColecAcessoNaoAprovado { get; }
    string NomeColecSolicitacaoExclusaoConta { get; }
    string NomeColecSolicitacaoExclusaoPresenca { get; }
    string NomeColecSolicitacaoNovaPresenca { get; }
    string NomeColecTabela { get; }
    string NomeColecColuna { get; }
    string NomeColecLogValor { get; }
    string NomeColecRegistro { get; }
    string LocalStoreKeyDoIdColaboradorLogado { get; }
    string HashPass { get; }
    SetgColaboradorAdmin ColaboradorAdmin { get; }
}