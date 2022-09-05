using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Presenca;

namespace HoraH.Domain.Interfaces.Business;
public interface IPresencaBusiness
{
    Task<BsnResult<List<BsnRelacaoDeHorasTrabalhadas>>> PesquisarAsync(BsnPesquisaDePresenca bsnPesquisa);
}