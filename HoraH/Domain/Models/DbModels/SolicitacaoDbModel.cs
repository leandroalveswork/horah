using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class SolicitacaoDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColaboradorSolicitador { get; set; }
    public string? IdTipoSolicitacao { get; set; }
    public string MotivoSolicitacao { get; set; } = "";
    public string? IdStatusSolicitacao { get; set; }
    public string? IdColaboradorAprovadorOuRejeitador { get; set; }
    public string MotivoAprovacaoOuRejeitacao { get; set; } = "";
}