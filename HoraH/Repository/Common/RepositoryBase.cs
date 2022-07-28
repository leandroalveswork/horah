using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository.Common;
public class RepositoryBase<T> : IRepositoryBase<T>
{
    protected readonly IDbClientAccessor _dbClientAccessor;
    protected readonly IAppConfiguration _appConfiguration;
    protected readonly IDbSessionAccessor _dbSessionAccessor;
    public RepositoryBase(IDbClientAccessor dbClientAccessor, IAppConfiguration appConfiguration, IDbSessionAccessor dbSessionAccessor)
    {
        _dbClientAccessor = dbClientAccessor;
        _appConfiguration = appConfiguration;
        _dbSessionAccessor = dbSessionAccessor;
    }

    protected virtual string GetNomeColecEntidade()
    {
        throw new NotImplementedException();
    }

    protected virtual Expression<Func<T, bool>> GetMatchesId(string id)
    {
        throw new NotImplementedException();
    }

    protected IMongoCollection<T> GetCollection()
    {
        _dbClientAccessor.ConnectIfNull();
        var client = _dbClientAccessor.DbClient;
        var database = client.GetDatabase(_appConfiguration.NomeBD);
        var collection = database.GetCollection<T>(GetNomeColecEntidade());
        return collection;
    }

    public async Task<List<T>> SelectAllAsync()
    {
        _dbClientAccessor.ConnectIfNull();
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            var entities = await GetCollection().Find(_ => true).ToListAsync();
            return entities;
        }
        else
        {
            var entities = await GetCollection().Find(session, _ => true).ToListAsync();
            return entities;
        }
    }

    public async Task<T?> SelectByIdAsync(string id)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            var entity = await GetCollection().Find(GetMatchesId(id)).FirstOrDefaultAsync();
            return entity;
        }
        else
        {
            var entity = await GetCollection().Find(session, GetMatchesId(id)).FirstOrDefaultAsync();
            return entity;
        }
    }

    public async Task InsertAsync(T entity)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            await GetCollection().InsertOneAsync(entity);
        }
        else
        {
            await GetCollection().InsertOneAsync(session, entity);
        }
    }

    public async Task UpdateAsync(string id, T entity)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            await GetCollection().ReplaceOneAsync(GetMatchesId(id), entity);
        }
        else
        {
            await GetCollection().ReplaceOneAsync(session, GetMatchesId(id), entity);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            await GetCollection().DeleteOneAsync(GetMatchesId(id));
        }
        else
        {
            await GetCollection().DeleteOneAsync(session, GetMatchesId(id));
        }
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            return await GetCollection().Find(GetMatchesId(id)).AnyAsync();
        }
        else
        {
            return await GetCollection().Find(session, GetMatchesId(id)).AnyAsync();
        }
    }
}