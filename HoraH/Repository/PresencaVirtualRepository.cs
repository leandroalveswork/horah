using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.DbModels;
using HoraH.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository;
public class PresencaVirtualRepository : RepositoryBase<PresencaVirtualDbModel>, IPresencaVirtualRepository
{
    public PresencaVirtualRepository(IDbClientAccessor dbClientAccessor,
        IAppConfiguration appConfiguration,
        IDbSessionAccessor dbSessionAccessor)
        : base(dbClientAccessor, appConfiguration, dbSessionAccessor)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _appConfiguration.NomeColecPresencaVirtual;
    }

    protected override Expression<Func<PresencaVirtualDbModel, bool>> GetMatchesId(string id)
    {
        return x => x.Id == id;
    }
}