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
    string NomeColecPermissaoAcessoPendente { get; }
    string NomeColecSolicitacaoNovaPresenca { get; }
    string NomeColecSolicitacaoAlteracaoPresenca { get; }
    string NomeColecSolicitacaoExclusaoPresenca { get; }
    string NomeColecLogValor { get; }
    string NomeColecRegistro { get; }
    string NomeColecAlteracaoRegistro { get; }
    string LocalStoreKeyDoIdColaboradorLogado { get; }
    string HashPass { get; }
    SetgColaboradorAdmin ColaboradorAdmin { get; }
}