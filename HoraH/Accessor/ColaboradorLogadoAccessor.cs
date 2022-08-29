using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Interfaces.Accessor;

namespace HoraH.Accessor;
public class ColaboradorLogadoAccessor : IColaboradorLogadoAccessor
{
    public BsnColaboradorLogado? ColaboradorLogado { get; set; }
}
