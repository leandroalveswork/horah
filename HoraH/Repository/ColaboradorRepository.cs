using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.DbModels;
using HoraH.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository;
public class ColaboradorRepository : RepositoryBase<ColaboradorDbModel>, IColaboradorRepository
{
    public ColaboradorRepository(IDbClientAccessor dbClientAccessor, IAppConfiguration appConfiguration, IDbSessionAccessor dbSessionAccessor)
        : base(dbClientAccessor, appConfiguration, dbSessionAccessor)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _appConfiguration.NomeColecColaborador;
    }

    protected override Expression<Func<ColaboradorDbModel, bool>> GetMatchesId(string id)
    {
        return x => x.Id == id;
    }

    public async Task<ColaboradorDbModel?> SelectByLoginAsync(string login)
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            var colaborador = await GetCollection().Find(x => x.Login == login).FirstOrDefaultAsync();
            return colaborador;
        }
        else
        {
            var colaborador = await GetCollection().Find(session, x => x.Login == login).FirstOrDefaultAsync();
            return colaborador;
        }
    }
}