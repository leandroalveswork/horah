using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Common;

namespace HoraH.Domain.Interfaces.Business;
public interface IColaboradorBusiness
{
    Task<BsnResult<BsnWrapperBase<BsnResultadoDeColaborador>>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa);
}