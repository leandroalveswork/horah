using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class RegistroDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdColaboradorInclusao { get; set; }
    public DateTime HoraInclusao { get; set; }
    public bool FoiExcluido { get; set; }
    public string? IdColaboradorExclusao { get; set; }
    public DateTime? HoraExclusao { get; set; }

    #region Modulo Solicitacao

    public bool InclusaoOriginouDeSolicitacao { get; set; }
    public string? IdSolicitacaoInclusaoOrigem { get; set; }
    public bool ExclusaoOriginouDeSolicitacao { get; set; }
    public string? IdSolicitacaoExclusaoOrigem { get; set; }
    
    #endregion
}