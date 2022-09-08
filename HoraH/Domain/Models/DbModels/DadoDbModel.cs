using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class DadoDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColuna { get; set; }
    public string? ValorSerializadoJson { get; set; }
    public bool EInclusao { get; set; }
    public string? IdRegistroSeInclusao { get; set; }
    public string? IdAlteracaoRegistroSeAlteracao { get; set; }
}