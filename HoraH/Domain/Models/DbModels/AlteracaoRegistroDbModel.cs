using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HoraH.Domain.Models.DbModels;
public class AlteracaoRegistroDbModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? IdRegistroAlterado { get; set; }
    public DateTime HoraAlteracao { get; set; }

    #region Modulo Solicitacao

    public bool EstaEsperandoAprovacao { get; set; }
    
    #endregion
}