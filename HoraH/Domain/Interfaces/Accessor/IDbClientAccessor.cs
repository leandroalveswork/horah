using MongoDB.Driver;

namespace HoraH.Domain.Interfaces.Accessor;
public interface IDbClientAccessor
{
    MongoClient? DbClient { get; set; }
    void ConnectIfNull();
}