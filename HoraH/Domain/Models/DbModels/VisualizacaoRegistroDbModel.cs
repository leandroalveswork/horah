using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class VisualizacaoRegistroDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdRegistroVisualizado { get; set; }
    public DateTime HoraVisualizacao { get; set; }
}