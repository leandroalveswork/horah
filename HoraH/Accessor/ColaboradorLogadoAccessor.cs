using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;

namespace HoraH.Accessor;
public class ColaboradorLogadoAccessor : IColaboradorLogadoAccessor
{
    public BsnColaboradorLogado? ColaboradorLogado { get; set; }
}