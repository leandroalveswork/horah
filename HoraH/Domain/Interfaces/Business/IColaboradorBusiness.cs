using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;

namespace HoraH.Domain.Interfaces.Business;
public interface IColaboradorBusiness
{
    Task<BsnResult<BsnWrapperDoResultadoDeColaborador>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa, int resultadosPorPagina);
    int ResultadosPorPaginaPadrao { get; }
}