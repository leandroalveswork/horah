using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class AlteracaoRegistroDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColaboradorAlteracao { get; set; }
    public string? IdRegistroAlterado { get; set; }
    public DateTime HoraAlteracao { get; set; }
}