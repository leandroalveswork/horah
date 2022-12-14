using System.Security.Cryptography;
using System.Text;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Models.Bsn.Funcionalidade;
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.DbModels;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace HoraH.Business;
public class AutorizacaoBusiness : IAutorizacaoBusiness
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IColaboradorLogadoAccessor _colaboradorLogadoAccessor;
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IAcessoRepository _acessoRepository;
    private readonly IUnitOfWork _uow;
    private readonly IGravadorLogBusiness _gravadorLogBusiness;
    public AutorizacaoBusiness(IAppConfiguration appConfiguration,
                               IColaboradorLogadoAccessor colaboradorLogadoAccessor,
                               IColaboradorRepository colaboradorRepository,
                               IAcessoRepository acessoRepository,
                               IUnitOfWork uow,
                               IGravadorLogBusiness gravadorLogBusiness)
    {
        _appConfiguration = appConfiguration;
        _colaboradorLogadoAccessor = colaboradorLogadoAccessor;
        _colaboradorRepository = colaboradorRepository;
        _acessoRepository = acessoRepository;
        _uow = uow;
        _gravadorLogBusiness = gravadorLogBusiness;
    }

    public string CriptografarSenha(string senha)
    {
        var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(_appConfiguration.HashPass));
        var bytesSenha = Encoding.UTF8.GetBytes(senha);
        var hash = hmacSha512.ComputeHash(bytesSenha);
        return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
    }

    public async Task<BsnResult<object>> LogarColaboradorAsync(BsnLogar bsnLogar)
    {
        var resultValidacaoNulos = bsnLogar.ValidarNulos();
        if (!resultValidacaoNulos.EstaOk)
        {
            return new BsnResult<object>() { EstaOk = false, Mensagem = resultValidacaoNulos.Mensagem };
        }
        var bsnReturn = BsnResult<object>.Ok;
        var transactionOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var colaborador = await _colaboradorRepository.SelectByLoginAsync(bsnLogar.Login);
            if (colaborador == null)
            {
                bsnReturn = BsnResult<object>.Erro("Login ou senha inv??lidos.");
                return;
            }
            var senhaTentadaCriptografada = CriptografarSenha(bsnLogar.Senha);
            if (colaborador.Senha != senhaTentadaCriptografada)
            {
                bsnReturn = BsnResult<object>.Erro("Login ou senha inv??lidos.");
                return;
            }
            if (!colaborador.EstaAtivo)
            {
                colaborador.EstaAtivo = true;
                await _colaboradorRepository.UpdateAsync(colaborador.Id, colaborador);
                await _gravadorLogBusiness.GravarAlteracaoAsync(colaborador, BsnColunaLiterais.ListarIdsColunasSingle("Colaborador", "EstaAtivo", typeof(ColaboradorDbModel)), colaborador.Id);
            }
            await _gravadorLogBusiness.GravarVisualizacaoAsync(colaborador, BsnColunaLiterais.ListarIdsColunasSingle("Colaborador", "Login", typeof(ColaboradorDbModel)), colaborador.Id);
            var acessosDoColaborador = await _acessoRepository.SelectByIdDoColaboradorAsync(colaborador.Id);
            await _gravadorLogBusiness.GravarMuitasVisualizacoesAsync(acessosDoColaborador, BsnColunaLiterais.ListarIdsColunas("Acesso", new [] { "Id", "IdColaborador", "IdFuncionalidade", "EstaPermitido" }, typeof(AcessoDbModel)), colaborador.Id);
            _colaboradorLogadoAccessor.ColaboradorLogado = new BsnColaboradorLogado()
            {
                Id = colaborador.Id,
                Nome = colaborador.Nome,
                Login = colaborador.Login,
                Acessos = acessosDoColaborador
                    .Select(x => new BsnAcesso
                    {
                        IdFuncionalidade = x.IdFuncionalidade,
                        Funcionalidade = BsnFuncionalidadeLiterais.GetById(x.IdFuncionalidade),
                        EstaPermitido = x.EstaPermitido
                    })
                    .ToList()
            };
        });
        if (!transactionOk)
        {
            return BsnResult<object>.Erro(Message.ErroNoServidor);
        }
        return bsnReturn;
    }

    public async Task<BsnResult<object>> IncluirColaboradorAsync(BsnNovoColaborador bsnNovoColaborador)
    {
        var resultValidacaoNulos = bsnNovoColaborador.ValidarNulos();
        if (!resultValidacaoNulos.EstaOk)
        {
            return BsnResult<object>.Erro(resultValidacaoNulos.Mensagem);
        }
        var resultValidacaoSenhaConfirmarSenhaIguais = bsnNovoColaborador.ValidarSenhaConfirmarSenhaIguais();
        if (!resultValidacaoNulos.EstaOk)
        {
            return BsnResult<object>.Erro(resultValidacaoSenhaConfirmarSenhaIguais.Mensagem);
        }
        var bsnReturn = BsnResult<object>.Ok;
        var transactionOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var resultValidacaoComMesmoLogin = await ValidarNovoColaboradorComMesmoLoginAsync(bsnNovoColaborador);
            if (!resultValidacaoComMesmoLogin.EstaOk)
            {
                bsnReturn = BsnResult<object>.Erro(resultValidacaoComMesmoLogin.Mensagem);
                return;
            }
            var novoDbColaborador = new ColaboradorDbModel
            {
                Id = MongoId.NewMongoId,
                Nome = bsnNovoColaborador.Nome,
                Login = bsnNovoColaborador.Login,
                Senha = CriptografarSenha(bsnNovoColaborador.Senha),
                EstaAtivo = true
            };
            await _colaboradorRepository.InsertAsync(novoDbColaborador);
            await _gravadorLogBusiness.GravarInclusaoAsync(novoDbColaborador, novoDbColaborador.Id);
            var acessosPadrao = _acessoRepository.MontarAcessosPadraoParaColaborador(novoDbColaborador.Id);
            foreach (var acessoPadrao in acessosPadrao)
            {
                await _acessoRepository.InsertAsync(acessoPadrao);
                await _gravadorLogBusiness.GravarInclusaoAsync(acessoPadrao, novoDbColaborador.Id);
            }
            _colaboradorLogadoAccessor.ColaboradorLogado = new BsnColaboradorLogado
            {
                Id = novoDbColaborador.Id,
                Nome = novoDbColaborador.Nome,
                Login = novoDbColaborador.Login,
                Acessos = acessosPadrao
                    .Select(x => new BsnAcesso
                    {
                        IdFuncionalidade = x.IdFuncionalidade,
                        Funcionalidade = BsnFuncionalidadeLiterais.GetById(x.IdFuncionalidade),
                        EstaPermitido = x.EstaPermitido
                    })
                    .ToList()
            };
        });
        if (!transactionOk)
        {
            return BsnResult<object>.Erro(Message.ErroNoServidor);
        }
        return bsnReturn;
    }

    public async Task<BsnResult<object>> ValidarNovoColaboradorComMesmoLoginAsync(BsnNovoColaborador bsnNovoColaborador)
    {
        var colaboradorComMesmoLogin = await _colaboradorRepository.SelectByLoginAsync(bsnNovoColaborador.Login);
        if (colaboradorComMesmoLogin != null)
        {
            return BsnResult<object>.Erro("J?? existe esse login.");
        }
        return BsnResult<object>.Ok;
    }

    public async Task<BsnResult<object>> AlterarColaboradorAsync(BsnAlterarConta bsnAlterarConta)
    {
        var resultValidacaoNulos = bsnAlterarConta.ValidarNulos();
        if (!resultValidacaoNulos.EstaOk)
        {
            return BsnResult<object>.Erro(resultValidacaoNulos.Mensagem);
        }
        var resultValidacaoSenhaConfirmarSenhaIguais = bsnAlterarConta.ValidarSenhaConfirmarSenhaIguais();
        if (!resultValidacaoNulos.EstaOk)
        {
            return BsnResult<object>.Erro(resultValidacaoSenhaConfirmarSenhaIguais.Mensagem);
        }
        var bsnReturn = BsnResult<object>.Ok;
        bsnReturn.Mensagem = Message.RegistroAlteradoSucesso;
        var transactionOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var resultValidacaoComMesmoLogin = await ValidarAlterarColaboradorComMesmoLoginAsync(bsnAlterarConta);
            if (!resultValidacaoComMesmoLogin.EstaOk)
            {
                bsnReturn = BsnResult<object>.Erro(resultValidacaoComMesmoLogin.Mensagem);
                return;
            }
            var alterarDbColaborador = await _colaboradorRepository.SelectByIdAsync(_colaboradorLogadoAccessor.ColaboradorLogado.Id);
            var dbColaboradorAntes = alterarDbColaborador.DuplicarNaMemoria();
            alterarDbColaborador.Nome = bsnAlterarConta.Nome;
            alterarDbColaborador.Login = bsnAlterarConta.Login;
            if (!string.IsNullOrWhiteSpace(bsnAlterarConta.Senha))
            {
                alterarDbColaborador.Senha = CriptografarSenha(bsnAlterarConta.Senha);
            }
            alterarDbColaborador.EstaAtivo = true;
            await _colaboradorRepository.UpdateAsync(alterarDbColaborador.Id, alterarDbColaborador);
            await _gravadorLogBusiness.GravarAlteracaoAutoDiffsAsync(dbColaboradorAntes, alterarDbColaborador, _colaboradorLogadoAccessor.ColaboradorLogado.Id);
            _colaboradorLogadoAccessor.ColaboradorLogado.Nome = bsnAlterarConta.Nome;
            _colaboradorLogadoAccessor.ColaboradorLogado.Login = bsnAlterarConta.Login;
        });
        if (!transactionOk)
        {
            return BsnResult<object>.Erro(Message.ErroNoServidor);
        }
        return bsnReturn;
    }

    public async Task<BsnResult<object>> ValidarAlterarColaboradorComMesmoLoginAsync(BsnAlterarConta bsnAlterarConta)
    {
        var colaboradorComMesmoLogin = await _colaboradorRepository.SelectByLoginAsync(bsnAlterarConta.Login);
        if (colaboradorComMesmoLogin != null && colaboradorComMesmoLogin.Id != _colaboradorLogadoAccessor.ColaboradorLogado.Id)
        {
            return BsnResult<object>.Erro("J?? existe esse login.");
        }
        return BsnResult<object>.Ok;
    }
}