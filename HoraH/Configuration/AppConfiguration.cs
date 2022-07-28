using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Settings;
using Microsoft.Extensions.Options;

namespace HoraH.Configuration;
public class AppConfiguration : IAppConfiguration
{
    private readonly IConfiguration _configuration;
    private readonly SetgColaboradorAdmin _setgColaboradorAdmin; 
    public AppConfiguration(IConfiguration configuration, IOptions<SetgColaboradorAdmin> optionsSetgColaboradorAdmin)
    {
        _configuration = configuration;
        _setgColaboradorAdmin = optionsSetgColaboradorAdmin.Value;
    }
    public string ConexaoBD => _configuration["CONEXAO_BANCO_DADOS_NOSQL"];
    public string NomeBD => _configuration["NOME_BANCO_DADOS_NOSQL"];
    public string NomeColecColaborador => _configuration["BancoDadosMongoDB:ColecColaborador"];
    public string NomeColecAcesso => _configuration["BancoDadosMongoDB:ColecAcesso"];
    public string NomeColecFuncionalidade => _configuration["BancoDadosMongoDB:ColecFuncionalidade"];
    public string NomeColecPresenca => _configuration["BancoDadosMongoDB:ColecPresenca"];
    public string NomeColecSolicitacaoAcesso => _configuration["BancoDadosMongoDB:ColecSolicitacaoAcesso"];
    public string NomeColecAcessoNaoAprovado => _configuration["BancoDadosMongoDB:ColecAcessoNaoAprovado"];
    public string NomeColecSolicitacaoExclusaoConta => _configuration["BancoDadosMongoDB:ColecSolicitacaoExclusaoConta"];
    public string NomeColecSolicitacaoExclusaoPresenca => _configuration["BancoDadosMongoDB:ColecSolicitacaoExclusaoPresenca"];
    public string NomeColecSolicitacaoNovaPresenca => _configuration["BancoDadosMongoDB:ColecSolicitacaoNovaPresenca"];
    public string NomeColecTabela => _configuration["BancoDadosMongoDB:ColecTabela"];
    public string NomeColecColuna => _configuration["BancoDadosMongoDB:ColecColuna"];
    public string NomeColecLogValor => _configuration["BancoDadosMongoDB:ColecLogValor"];
    public string NomeColecRegistro => _configuration["BancoDadosMongoDB:ColecRegistro"];
    public string LocalStoreKeyDoIdColaboradorLogado => "IdColaboradorLogado";
    public string HashPass => _configuration["HashPass"];
    public SetgColaboradorAdmin ColaboradorAdmin => _setgColaboradorAdmin;
}