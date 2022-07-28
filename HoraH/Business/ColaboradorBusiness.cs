using HoraH.Domain.Design.Patterns.Strategy;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;

namespace HoraH.Business;
public class ColaboradorBusiness : IColaboradorBusiness
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IColaboradorLogadoAccessor _colaboradorLogadoAccessor;
    private readonly IColaboradorRepository _colaboradorRepository;
    public ColaboradorBusiness(IAppConfiguration appConfiguration,
                               IColaboradorLogadoAccessor colaboradorLogadoAccessor,
                               IColaboradorRepository colaboradorRepository)
    {
        _appConfiguration = appConfiguration;
        _colaboradorLogadoAccessor = colaboradorLogadoAccessor;
        _colaboradorRepository = colaboradorRepository;
    }

    public async Task<BsnResult<BsnWrapperDoResultadoDeColaborador>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa, int resultadosPorPagina)
    {
        var colaboradores = await _colaboradorRepository.SelectAllAsync();
        var resultadosDeColaborador = HrhFiltrador.FiltrarPeloTexto(colaboradores, x => x.Nome.ToLower().RemoverAcentuacao(), bsnPesquisa.Nome?.ToLower()?.RemoverAcentuacao());
        resultadosDeColaborador = HrhFiltrador.FiltrarPeloTexto(resultadosDeColaborador, x => x.Login.ToLower().RemoverAcentuacao(), bsnPesquisa.Login?.ToLower()?.RemoverAcentuacao());
        resultadosDeColaborador = HrhFiltrador.FiltrarPeloPredicate(resultadosDeColaborador, x => x.EstaAtivo == bsnPesquisa.EstaAtivo, bsnPesquisa.EstaAtivo);
        resultadosDeColaborador = HrhOrdenador.OrdenarBaseadoNoNumeroDaColuna(resultadosDeColaborador, bsnPesquisa.NumeroDaColuna, bsnPesquisa.EhCrescente, x => x.Nome, x => x.Login, x => x.EstaAtivo);
        var totalDeResultadosDeColaborador = resultadosDeColaborador.Count();
        resultadosDeColaborador = HrhPaginador.Paginar(resultadosDeColaborador, bsnPesquisa.NumeroDaPagina, resultadosPorPagina, out int numeroDaPaginaCorrigido);
        return new BsnResult<BsnWrapperDoResultadoDeColaborador>()
        {
            EstaOk = true,
            Resultado = new BsnWrapperDoResultadoDeColaborador()
            {
                Resultados = new List<BsnResultadoDeColaborador>(resultadosDeColaborador.Select(colb => new BsnResultadoDeColaborador()
                {
                    Id = colb.Id,
                    Nome = colb.Nome,
                    Login = colb.Login,
                    EstaAtivo = colb.EstaAtivo
                })),
                Total = totalDeResultadosDeColaborador,
                NumeroDaPaginaCorrigido = numeroDaPaginaCorrigido
            }
        };
    }

    public int ResultadosPorPaginaPadrao => 5;
}