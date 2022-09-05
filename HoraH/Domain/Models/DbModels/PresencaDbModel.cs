using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class PresencaDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColaborador { get; set; }
    public string? IdEvento { get; set; }
    public DateTime HoraMarcada { get; set; }
}