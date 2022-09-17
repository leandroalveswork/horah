using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnVisualizacaoGravada
{
    public List<DadoDbModel> DadosExtraidos { get; set; }
    public string ObterIdSerializadoJson(BsnColuna colunaEId)
    {
        return DadosExtraidos.First(x => x.IdColuna == colunaEId.Id).ValorSerializadoJson;
    }
    public DadoDbModel DadoEIdDb { get; set; }
}