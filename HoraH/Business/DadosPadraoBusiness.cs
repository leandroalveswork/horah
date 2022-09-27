using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Funcionalidade;
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Business;
public class DadosPadraoBusiness : IDadosPadraoBusiness
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IAcessoRepository _acessoRepository;
    private readonly IUnitOfWork _uow;
    private readonly IAutorizacaoBusiness _autorizacaoBusiness;
    private readonly IGravadorLogBusiness _gravadorLogBusiness;
    public DadosPadraoBusiness(IAppConfiguration appConfiguration,
                               IColaboradorRepository colaboradorRepository,
                               IAcessoRepository acessoRepository,
                               IUnitOfWork uow,
                               IAutorizacaoBusiness autorizacaoBusiness,
                               IGravadorLogBusiness gravadorLogBusiness)
    {
        _appConfiguration = appConfiguration;
        _colaboradorRepository = colaboradorRepository;
        _acessoRepository = acessoRepository;
        _uow = uow;
        _autorizacaoBusiness = autorizacaoBusiness;
        _gravadorLogBusiness = gravadorLogBusiness;
    }

    private async Task CompletarAcessosDoAdminAsync(string idDoAdmin)
    {
        var acessosDoAdminDb = await _acessoRepository.SelectByIdDoColaboradorAsync(idDoAdmin);
        foreach (var funcionalidadeDoSistema in BsnFuncionalidadeLiterais.ListarTodos())
        {
            var acessoAFuncionalidade = acessosDoAdminDb.FirstOrDefault(x => x.IdFuncionalidade == funcionalidadeDoSistema.Id);
            if (acessoAFuncionalidade == null)
            {
                var inserirAcessoDb = new AcessoDbModel()
                {
                    Id = MongoId.NewMongoId,
                    IdColaborador = idDoAdmin,
                    IdFuncionalidade = funcionalidadeDoSistema.Id,
                    EstaPermitido = true
                };
                await _acessoRepository.InsertAsync(inserirAcessoDb);
                await _gravadorLogBusiness.GravarInclusaoAsync(inserirAcessoDb, idDoAdmin);
                continue;
            }
            if (!acessoAFuncionalidade.EstaPermitido)
            {
                acessoAFuncionalidade.EstaPermitido = true;
                await _acessoRepository.UpdateAsync(acessoAFuncionalidade.Id, acessoAFuncionalidade);
                var idColunaEstaPermitido = BsnColunaLiterais.ListarIdsColunasSingle("Acesso", "EstaPermitido", typeof(AcessoDbModel));
                await _gravadorLogBusiness.GravarAlteracaoAsync(acessoAFuncionalidade, idColunaEstaPermitido, idDoAdmin);
            }
        }
    }

    private async Task CompletarAdminAsync()
    {
        var colaboradorAdminBanco = await _colaboradorRepository.SelectByLoginAsync(_appConfiguration.ColaboradorAdmin.Login);
        var senhaDoAdminCriptografada = _autorizacaoBusiness.CriptografarSenha(_appConfiguration.ColaboradorAdmin.SenhaRaw);
        if (colaboradorAdminBanco == null)
        {
            var inserirAdminDb = new ColaboradorDbModel()
            {
                Id = MongoId.NewMongoId,
                Nome = _appConfiguration.ColaboradorAdmin.Nome,
                Login = _appConfiguration.ColaboradorAdmin.Login,
                Senha = senhaDoAdminCriptografada,
                EstaAtivo = true
            };
            await _colaboradorRepository.InsertAsync(inserirAdminDb);
            await _gravadorLogBusiness.GravarInclusaoAsync(inserirAdminDb, inserirAdminDb.Id);
            await CompletarAcessosDoAdminAsync(inserirAdminDb.Id);
            return;
        }
        if (colaboradorAdminBanco.Nome != _appConfiguration.ColaboradorAdmin.Nome
            || colaboradorAdminBanco.Login != _appConfiguration.ColaboradorAdmin.Login
            || colaboradorAdminBanco.Senha != senhaDoAdminCriptografada
            || !colaboradorAdminBanco.EstaAtivo)
        {
            var colaboradorAntesSalvar = colaboradorAdminBanco.DuplicarNaMemoria();
            colaboradorAdminBanco.Nome = _appConfiguration.ColaboradorAdmin.Nome;
            colaboradorAdminBanco.Login = _appConfiguration.ColaboradorAdmin.Login;
            colaboradorAdminBanco.Senha = senhaDoAdminCriptografada;
            colaboradorAdminBanco.EstaAtivo = true;
            await _colaboradorRepository.UpdateAsync(colaboradorAdminBanco.Id, colaboradorAdminBanco);
            await _gravadorLogBusiness.GravarAlteracaoAutoDiffsAsync(colaboradorAntesSalvar, colaboradorAdminBanco, colaboradorAdminBanco.Id);
        }
        await CompletarAcessosDoAdminAsync(colaboradorAdminBanco.Id);
    }

    public async Task CompletarAsync()
    {
        await _uow.ExecuteTransactionAndReturnOkAsync(CompletarAdminAsync);
    }
}