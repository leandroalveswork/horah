using HoraH.Domain.Interfaces.Accessor;
using MongoDB.Driver;

namespace HoraH.Accessor;
public class DbSessionAccessor : IDbSessionAccessor
{
    public IClientSessionHandle? DbSession { get; set; }
}