using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class ColaboradorDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Nome { get; set; } = "";
    public string Login { get; set; } = "";
    public string Senha { get; set; } = "";
    public bool EstaAtivo { get; set; }
}