using HoraH.Domain.Models.Bsn.Funcionalidade;

namespace HoraH.Domain.Models.Bsn.Autorizacao;
public class BsnAcesso
{
    public string? IdFuncionalidade { get; set; }
    public BsnFuncionalidade Funcionalidade { get; set; }
    public bool EstaPermitido { get; set; }
}