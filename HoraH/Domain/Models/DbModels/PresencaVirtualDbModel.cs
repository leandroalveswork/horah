using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class PresencaVirtualDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColaboradorMarcador { get; set; }
    public string? IdEvento { get; set; }
    public DateTime HoraMarcada { get; set; }
    public string? IdSolicitacaoOrigem { get; set; }
    public string? IdPresencaManipulada { get; set; }
}