using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Models.DbModels;
using HoraH.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository;
public class AcessoRepository : RepositoryBase<AcessoDbModel>, IAcessoRepository
{
    private readonly IFuncionalidadeRepository _funcionalidadeRepository;
    public AcessoRepository(IDbClientAccessor dbClientAccessor,
                            IAppConfiguration appConfiguration,
                            IDbSessionAccessor dbSessionAccessor,
                            IFuncionalidadeRepository funcionalidadeRepository)
        : base(dbClientAccessor, appConfiguration, dbSessionAccessor)
    {
        _funcionalidadeRepository = funcionalidadeRepository;
    }

    protected override string GetNomeColecEntidade()
    {
        return _appConfiguration.NomeColecAcesso;
    }

    protected override Expression<Func<AcessoDbModel, bool>> GetMatchesId(string id)
    {
        return x => x.Id == id;
    }

    public async Task<List<AcessoDbModel>> SelectByIdDoColaboradorAsync(string idDoColaborador)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            var acessos = await GetCollection().Find(x => x.IdColaborador == idDoColaborador).ToListAsync();
            return acessos;
        }
        else
        {
            var acessos = await GetCollection().Find(session, x => x.IdColaborador == idDoColaborador).ToListAsync();
            return acessos;
        }
    }

    // Acessos padrão podem apenas marcar presença e solicitar
    public List<AcessoDbModel> MontarAcessosPadraoParaColaborador(string idColaborador)
    {
        return new List<AcessoDbModel>()
        {
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.ColaboradorListar), EstaPermitido = false },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.ColaboradorAcessoAlterar), EstaPermitido = false },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.ColaboradorAtivar), EstaPermitido = false },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.RegistroListar), EstaPermitido = false },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.PresencaListar), EstaPermitido = false },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.PresencaMarcar), EstaPermitido = true },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.SolicitacaoIncluir), EstaPermitido = true },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.SolicitacaoListar), EstaPermitido = false },
            new AcessoDbModel() { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = _funcionalidadeRepository.GetIdDaFuncionalidadeComNome(BsnFuncionalidadeDoSistema.SolicitacaoAprovar), EstaPermitido = false },
        };
    }
}