using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace HoraH.Domain.Interfaces.Business;
public interface IAutorizacaoBusiness
{
    Task<BsnResult<object>> LogarColaboradorAsync(BsnLogar bsnLogar);
    string CriptografarSenha(string senha);
    Task<BsnResult<object>> IncluirColaboradorAsync(BsnNovoColaborador bsnNovoColaborador);
    Task<BsnResult<object>> ValidarNovoColaboradorComMesmoLoginAsync(BsnNovoColaborador bsnNovoColaborador);
    Task<BsnResult<object>> AlterarColaboradorAsync(BsnAlterarConta bsnAlterarConta);
}