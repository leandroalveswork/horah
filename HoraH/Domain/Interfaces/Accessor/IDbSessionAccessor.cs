using MongoDB.Driver;

namespace HoraH.Domain.Interfaces.Accessor;
public interface IDbSessionAccessor
{
    IClientSessionHandle? DbSession { get; set; }
}