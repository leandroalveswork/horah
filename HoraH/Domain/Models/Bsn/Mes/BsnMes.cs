using HoraH.Domain.Models.Bsn.Presenca;

namespace HoraH.Domain.Models.Bsn.Mes;
public class BsnMes
{
    public string Id { get; set; } = "";
    public string Sigla { get; set; } = "";
    public string Nome { get; set; } = "";
    public int ObterMonthComecandoUm()
    {
        if (!int.TryParse(Id, out int idAsInt))
        {
            return -1;
        }
        return idAsInt;
    }
}