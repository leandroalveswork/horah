using HoraH.Domain.Design.Patterns.Strategy;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Common;

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

    public async Task<BsnResult<List<BsnRelacaoDeColaborador>>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa)
    {
        var colaboradores = await _colaboradorRepository.SelectAllAsync();
        var relacoesDeColaborador = HrhFiltradorAnulavel.FiltrarPeloTexto(colaboradores, x => x.Nome.ToLower().RemoverAcentuacao(), bsnPesquisa.Nome?.ToLower()?.RemoverAcentuacao());
        relacoesDeColaborador = HrhFiltradorAnulavel.FiltrarPeloTexto(relacoesDeColaborador, x => x.Login.ToLower().RemoverAcentuacao(), bsnPesquisa.Login?.ToLower()?.RemoverAcentuacao());
        relacoesDeColaborador = HrhFiltradorAnulavel.FiltrarPeloPredicate(relacoesDeColaborador, x => x.EstaAtivo == bsnPesquisa.EstaAtivo, bsnPesquisa.EstaAtivo);
        return BsnResult<List<BsnRelacaoDeColaborador>>.OkConteudo(
            relacoesDeColaborador
                .Select(colb => new BsnRelacaoDeColaborador
                {
                    Id = colb.Id,
                    Nome = colb.Nome,
                    Login = colb.Login,
                    EstaAtivo = colb.EstaAtivo
                })
                .ToList()
        );
    }

    public async Task<BsnResult<object>> AtivarAsync(string id)
    {
        var dbColaborador = await _colaboradorRepository.SelectByIdAsync(id);
        if (dbColaborador == null)
        {
            return BsnResult<object>.Erro("Colaborador não encontrado.");
        }
        var resultOk = BsnResult<object>.Ok;
        resultOk.Mensagem = Message.RegistroAtivadoSucesso;
        if (dbColaborador.EstaAtivo)
        {
            return resultOk;
        }
        dbColaborador.EstaAtivo = true;
        await _colaboradorRepository.UpdateAsync(id, dbColaborador);
        return resultOk;
    }

    public async Task<BsnResult<object>> InativarAsync(string id)
    {
        var dbColaborador = await _colaboradorRepository.SelectByIdAsync(id);
        if (dbColaborador == null)
        {
            return BsnResult<object>.Erro("Colaborador não encontrado.");
        }
        var resultOk = BsnResult<object>.Ok;
        resultOk.Mensagem = Message.RegistroInativadoSucesso;
        if (!dbColaborador.EstaAtivo)
        {
            return resultOk;
        }
        dbColaborador.EstaAtivo = false;
        await _colaboradorRepository.UpdateAsync(id, dbColaborador);
        return resultOk;
    }
}