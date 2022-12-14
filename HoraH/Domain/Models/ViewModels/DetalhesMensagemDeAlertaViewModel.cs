using Microsoft.AspNetCore.Components;

namespace HoraH.Domain.Models.ViewModels;
public class DetalhesMensagemDeAlertaViewModel
{
    public bool EstaAberto { get; set; } = false;
    public string Titulo { get; set; } = "";
    public string NomeDaClasseDeCssDoIcone { get; set; } = "";
    public string Mensagem { get; set; } = "";
    public EventCallback? AcaoAposFechar { get; set; }
}