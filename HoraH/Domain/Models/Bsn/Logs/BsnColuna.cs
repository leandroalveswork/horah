using System.Text.Json;

namespace HoraH.Domain.Models.Bsn.Logs;
public class BsnColuna
{
    public string Id { get; set; } = "";
    public string Nome { get; set; } = "";
    public string NomeTabela { get; set; } = "";
    public string NomeColuna { get; set; } = "";
    // public Type TipoOriginal { get; set; }
}