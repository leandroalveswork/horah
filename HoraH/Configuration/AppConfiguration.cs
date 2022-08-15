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
    public string NomeColecColaborador => "Colaborador";
    public string NomeColecAcesso => "Acesso";
    public string NomeColecFuncionalidade => "Funcionalidade";
    public string NomeColecPresenca => "Presenca";
    public string NomeColecSolicitacaoAcesso => "SolicitacaoAcesso";
    public string NomeColecPermissaoAcessoPendente => "PermissaoAcessoPendente";
    public string NomeColecSolicitacaoNovaPresenca => "SolicitacaoNovaPresenca";
    public string NomeColecSolicitacaoAlteracaoPresenca => "SolicitacaoAlteracaoPresenca";
    public string NomeColecSolicitacaoExclusaoPresenca => "SolicitacaoExclusaoPresenca";
    public string NomeColecLogValor => "LogValor";
    public string NomeColecRegistro => "Registro";
    public string NomeColecAlteracaoRegistro => "AlteracaoRegistro";
    public string LocalStoreKeyDoIdColaboradorLogado => "IdColaboradorLogado";
    public string HashPass => _configuration["HashPass"];
    public SetgColaboradorAdmin ColaboradorAdmin => _setgColaboradorAdmin;
}