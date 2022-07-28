using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class AcessoDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColaborador { get; set; }
    public string? IdFuncionalidade { get; set; }
    public bool EstaPermitido { get; set; }
}