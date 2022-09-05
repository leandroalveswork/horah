using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Models.Bsn.Funcionalidade;
using HoraH.Domain.Models.DbModels;
using HoraH.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository;
public class AcessoRepository : RepositoryBase<AcessoDbModel>, IAcessoRepository
{
    public AcessoRepository(IDbClientAccessor dbClientAccessor,
                            IAppConfiguration appConfiguration,
                            IDbSessionAccessor dbSessionAccessor)
        : base(dbClientAccessor, appConfiguration, dbSessionAccessor)
    {
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
        return new List<AcessoDbModel>
        {
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.ListarColaborador.Id, EstaPermitido = false },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.ListarAcesso.Id, EstaPermitido = false },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.AlterarAcesso.Id, EstaPermitido = false },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.AtivarColaborador.Id, EstaPermitido = false },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.ListarLog.Id, EstaPermitido = false },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.MarcarPresenca.Id, EstaPermitido = false },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.ListarPresenca.Id, EstaPermitido = true },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.IncluirSolicitacao.Id, EstaPermitido = true },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.ListarSolicitacao.Id, EstaPermitido = false },
            new AcessoDbModel { Id = MongoId.NewMongoId, IdColaborador = idColaborador, IdFuncionalidade = BsnFuncionalidadeLiterais.AprovarSolicitacao.Id, EstaPermitido = false },
        };
    }
}