using HoraH.Domain.Design.Patterns.Strategy;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Funcionalidade;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Business;
public class ColaboradorBusiness : IColaboradorBusiness
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IColaboradorLogadoAccessor _colaboradorLogadoAccessor;
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IAcessoRepository _acessoRepository;
    private readonly IUnitOfWork _uow;
    public ColaboradorBusiness(IAppConfiguration appConfiguration,
                               IColaboradorLogadoAccessor colaboradorLogadoAccessor,
                               IColaboradorRepository colaboradorRepository,
                               IAcessoRepository acessoRepository,
                               IUnitOfWork uow)
    {
        _appConfiguration = appConfiguration;
        _colaboradorLogadoAccessor = colaboradorLogadoAccessor;
        _colaboradorRepository = colaboradorRepository;
        _acessoRepository = acessoRepository;
        _uow = uow;
    }

    public async Task<BsnResult<List<BsnRelacaoDeColaborador>>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa)
    {
        var colaboradores = await _colaboradorRepository.SelectAllAsync();
        var relacoesDeColaborador = HrhFiltradorAnulavel.FiltrarPeloTexto(colaboradores, x => x.Nome.ToLower().Trim().RemoverAcentuacao(), bsnPesquisa.Nome?.ToLower()?.Trim()?.RemoverAcentuacao());
        relacoesDeColaborador = HrhFiltradorAnulavel.FiltrarPeloTexto(relacoesDeColaborador, x => x.Login.ToLower().Trim().RemoverAcentuacao(), bsnPesquisa.Login?.ToLower()?.Trim()?.RemoverAcentuacao());
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

    public async Task<BsnResult<BsnRelacaoDeColaborador>> ObterPorIdAsync(string id)
    {
        var dbColaborador = await _colaboradorRepository.SelectByIdAsync(id);
        if (dbColaborador == null)
        {
            return BsnResult<BsnRelacaoDeColaborador>.Erro("Colaborador não encontrado.");
        }
        return BsnResult<BsnRelacaoDeColaborador>.OkConteudo(new BsnRelacaoDeColaborador
        {
            Id = dbColaborador.Id,
            Nome = dbColaborador.Nome,
            Login = dbColaborador.Login,
            EstaAtivo = dbColaborador.EstaAtivo
        });
    }

    public async Task<BsnResult<List<BsnAcesso>>> ObterAcessosDoColaboradorAsync(string idColaborador)
    {
        var dbColaborador = await _colaboradorRepository.SelectByIdAsync(idColaborador);
        if (dbColaborador == null)
        {
            return BsnResult<List<BsnAcesso>>.Erro("Colaborador não encontrado.");
        }
        var acessos = (await _acessoRepository.SelectByIdDoColaboradorAsync(idColaborador))
            .Select(x => new BsnAcesso
            {
                IdFuncionalidade = x.IdFuncionalidade,
                Funcionalidade = BsnFuncionalidadeLiterais.GetById(x.IdFuncionalidade),
                EstaPermitido = x.EstaPermitido
            }).ToList();
        return BsnResult<List<BsnAcesso>>.OkConteudo(acessos);
    }

    public async Task<BsnResult<object>> AlterarAcessosAsync(string idColaborador, List<BsnAcesso> acessos)
    {
        var transactionOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var acessosDb = await _acessoRepository.SelectByIdDoColaboradorAsync(idColaborador);
            foreach (var iAcessoIn in acessos)
            {
                var acessoDb = acessosDb.FirstOrDefault(x => x.IdFuncionalidade == iAcessoIn.IdFuncionalidade);
                if (acessoDb == null)
                {
                    await _acessoRepository.InsertAsync(new AcessoDbModel
                    {
                        Id = MongoId.NewMongoId,
                        IdColaborador = idColaborador,
                        IdFuncionalidade = iAcessoIn.IdFuncionalidade,
                        EstaPermitido = iAcessoIn.EstaPermitido
                    });
                    continue;
                }
                acessoDb.EstaPermitido = iAcessoIn.EstaPermitido;
                await _acessoRepository.UpdateAsync(acessoDb.Id, acessoDb);
            }
        });
        if (!transactionOk)
        {
            return BsnResult<object>.Erro(Message.ErroNoServidor);
        }
        var resOk = BsnResult<object>.Ok;
        resOk.Mensagem = Message.RegistroAlteradoSucesso;
        return resOk;
    }
}