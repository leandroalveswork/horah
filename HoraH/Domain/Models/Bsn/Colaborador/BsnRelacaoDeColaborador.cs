using HoraH.Domain.Models.Bsn.Args;
using Microsoft.AspNetCore.Components;

namespace HoraH.Domain.Models.Bsn.Colaborador;
public class BsnRelacaoDeColaborador
{
    public string Id { get; set; } = "";
    public string Nome { get; set; } = "";
    public string Login { get; set; } = "";
    public bool EstaAtivo { get; set; }
    public bool EstaAtivoPrompt
    {
        get
        {
            return EstaAtivo;
        }
        set
        {
            OnConfirmarPromptEstaAtivo.InvokeAsync(new BsnArgsPromptEstaAtivoColaborador
            {
                Id = Id,
                EstaAtivoAtual = value
            });
        }
    }
    public EventCallback<BsnArgsPromptEstaAtivoColaborador> OnConfirmarPromptEstaAtivo { get; set; }
}