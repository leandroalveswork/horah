namespace HoraH.Domain.Models.Bsn.Autorizacao;
public class BsnAcessoDoColaboradorLogado
{
    public string? IdFuncionalidade { get; set; }
    public string? NomeFuncionalidade { get; set; }
    public bool EstaPermitido { get; set; }
}