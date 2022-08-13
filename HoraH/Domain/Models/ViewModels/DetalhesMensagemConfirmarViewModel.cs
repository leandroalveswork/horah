using Microsoft.AspNetCore.Components;

namespace HoraH.Domain.Models.ViewModels;
public class DetalhesMensagemConfirmarViewModel
{
    public bool EstaAberto { get; set; } = false;
    public string Titulo { get; set; } = "";
    public string NomeDaClasseDeCssDoIcone { get; set; } = "";
    public string Mensagem { get; set; } = "";
    public string LabelConfirmar { get; set; } = "Sim";
    public string LabelCancelar { get; set; } = "Não";
    public EventCallback AcaoAposConfirmar { get; set; }
    public EventCallback? AcaoAposCancelar { get; set; }
}