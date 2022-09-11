using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class ItemSolicitacaoDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdSolicitacao { get; set; }
    public bool EstaAprovandoAlteracaoRegistro { get; set; }
    public string? IdRegistroOuAlteracaoRegistro { get; set; }
}