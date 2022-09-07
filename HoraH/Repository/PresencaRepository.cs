using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.DbModels;
using HoraH.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository;
public class PresencaRepository : RepositoryBase<PresencaDbModel>, IPresencaRepository
{
    public PresencaRepository(IDbClientAccessor dbClientAccessor,
        IAppConfiguration appConfiguration,
        IDbSessionAccessor dbSessionAccessor)
        : base(dbClientAccessor, appConfiguration, dbSessionAccessor)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _appConfiguration.NomeColecPresenca;
    }

    protected override Expression<Func<PresencaDbModel, bool>> GetMatchesId(string id)
    {
        return x => x.Id == id;
    }

    public async Task<List<PresencaDbModel>> SelectEntreColaboradoresEPorEventoAsync(List<string> idsColaborador, string? idEvento)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            var presencas = await GetCollection().Find(x => idsColaborador.Contains(x.IdColaborador) && x.IdEvento == idEvento).ToListAsync();
            return presencas;
        }
        else
        {
            var presencas = await GetCollection().Find(session, x => idsColaborador.Contains(x.IdColaborador) && x.IdEvento == idEvento).ToListAsync();
            return presencas;
        }
    }

    public async Task<List<PresencaDbModel>> SelectPorEventoAsync(string? idEvento)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            var presencas = await GetCollection().Find(x => x.IdEvento == idEvento).ToListAsync();
            return presencas;
        }
        else
        {
            var presencas = await GetCollection().Find(session, x => x.IdEvento == idEvento).ToListAsync();
            return presencas;
        }
    }
}