using HoraH.Domain.Models.Bsn.Args;
using HoraH.Domain.Models.Bsn.Evento;
using HoraH.Domain.Models.Bsn.Mes;
using Microsoft.AspNetCore.Components;

namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnRelacaoDePresenca
{
    public string Id { get; set; } = "";
    public string IdColaborador { get; set; } = "";
    public string NomeColaborador { get; set; } = "";
    public string IdEvento { get; set; } = "";
    public DateTime HoraMarcada { get; set; }
    public string EventoView => BsnEventoLiterais.GetById(IdEvento).Nome;
    public string HoraView => HoraMarcada.ToString("HH:mm:ss");
    public string DiaView => HoraMarcada.ToString("dd/MM/yyyy");
}