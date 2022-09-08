using HoraH.Domain.Models.Settings;

namespace HoraH.Domain.Interfaces.Configuration;
public interface IAppConfiguration
{
    string ConexaoBD { get; }
    string NomeBD { get; }
    string NomeColecColaborador { get; }
    string NomeColecAcesso { get; }
    string NomeColecPresenca { get; }
    string NomeColecDado { get; }
    string NomeColecRegistro { get; }
    string NomeColecAlteracaoRegistro { get; }
    string NomeColecSolicitacao { get; }
    string LocalStoreKeyDoIdColaboradorLogado { get; }
    string HashPass { get; }
    SetgColaboradorAdmin ColaboradorAdmin { get; }
}