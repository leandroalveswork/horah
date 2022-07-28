using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;

namespace HoraH.Domain.Interfaces.Accessor;
public interface IColaboradorLogadoAccessor
{
    BsnColaboradorLogado? ColaboradorLogado { get; set; }
}