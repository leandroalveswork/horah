using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.DbModels;
using HoraH.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository;
public class ItemSolicitacaoRepository : RepositoryBase<ItemSolicitacaoDbModel>, IItemSolicitacaoRepository
{
    public ItemSolicitacaoRepository(IDbClientAccessor dbClientAccessor,
        IAppConfiguration appConfiguration,
        IDbSessionAccessor dbSessionAccessor)
        : base(dbClientAccessor, appConfiguration, dbSessionAccessor)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _appConfiguration.NomeColecItemSolicitacao;
    }

    protected override Expression<Func<ItemSolicitacaoDbModel, bool>> GetMatchesId(string id)
    {
        return x => x.Id == id;
    }
}