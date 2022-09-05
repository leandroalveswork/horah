using HoraH.Domain.Models.Bsn.Funcionalidade;

namespace HoraH.Domain.Models.Bsn.Autorizacao;
public class BsnColaboradorLogado
{
    public string Id { get; set; } = "";
    public string Nome { get; set; } = "";
    public string Login { get; set; } = "";
    public bool EstaLogado => !string.IsNullOrWhiteSpace(Id);
    public List<BsnAcesso> Acessos { get; set; } = new List<BsnAcesso>();
    public bool TemPermissaoPara(BsnFuncionalidade funcionalidade)
    {
        return Acessos.Any(x => x.EstaPermitido && x.IdFuncionalidade == funcionalidade.Id);
    }
}