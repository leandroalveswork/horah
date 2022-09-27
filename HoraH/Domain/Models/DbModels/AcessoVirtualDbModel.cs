using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class AcessoVirtualDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColaboradorComAcesso { get; set; }
    public string? IdFuncionalidade { get; set; }
    public bool EstaPermitido { get; set; }
    public string? IdSolicitacaoOrigem { get; set; }
    public string? IdAcessoManipulado { get; set; }
}