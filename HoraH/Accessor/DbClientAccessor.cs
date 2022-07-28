using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using MongoDB.Driver;

namespace HoraH.Accessor;
public class DbClientAccessor : IDbClientAccessor
{
    private readonly IAppConfiguration _appConfiguration;
    public DbClientAccessor(IAppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }
    
    public MongoClient? DbClient { get; set; }

    public void ConnectIfNull()
    {
        if (DbClient == null)
        {
            DbClient = new MongoClient(_appConfiguration.ConexaoBD);
        }
    }
}