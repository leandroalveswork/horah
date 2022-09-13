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
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.DbModels;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Business;
public class ColaboradorBusiness : IColaboradorBusiness
{
    private readonly IAppConfiguration _appConfiguration;
    private readonly IColaboradorLogadoAccessor _colaboradorLogadoAccessor;
    private readonly IDbSessionAccessor _dbSessionAccessor;
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IAcessoRepository _acessoRepository;
    private readonly IUnitOfWork _uow;
    private readonly IGravadorLogBusiness _gravadorLogsBusiness;
    public ColaboradorBusiness(IAppConfiguration appConfiguration,
                               IColaboradorLogadoAccessor colaboradorLogadoAccessor,
                               IDbSessionAccessor dbSessionAccessor,
                               IColaboradorRepository colaboradorRepository,
                               IAcessoRepository acessoRepository,
                               IUnitOfWork uow,
                               IGravadorLogBusiness gravadorLogBusiness)
    {
        _appConfiguration = appConfiguration;
        _colaboradorLogadoAccessor = colaboradorLogadoAccessor;
        _dbSessionAccessor = dbSessionAccessor;
        _colaboradorRepository = colaboradorRepository;
        _acessoRepository = acessoRepository;
        _uow = uow;
        _gravadorLogsBusiness = gravadorLogBusiness;
    }

    public async Task<BsnResult<List<BsnRelacaoDeColaborador>>> PesquisarAsync(BsnPesquisaDeColaborador bsnPesquisa)
    {
        var conteudoOk = new List<BsnRelacaoDeColaborador>();
        var uowSession = _dbSessionAccessor.DbSession;
        if (uowSession == null)
        {
            var transacaoFoiOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
                var linqExpFiltro = new LinqExpModel<ColaboradorDbModel>();
                if (bsnPesquisa.EstaAtivo.HasValue)
                {
                    linqExpFiltro.AppendAndAlso(x => x.EstaAtivo == bsnPesquisa.EstaAtivo.Value);
                }
                var colaboradoresDb = await _colaboradorRepository.SelectByLinqExpModelAsync(linqExpFiltro);
                if (!string.IsNullOrWhiteSpace(bsnPesquisa.Nome))
                {
                    colaboradoresDb = colaboradoresDb
                        .Where(x => x.Nome.ToLower().Trim().RemoverAcentuacao().Contains(bsnPesquisa.Nome.ToLower().Trim().RemoverAcentuacao()))
                        .ToList();
                }
                if (!string.IsNullOrWhiteSpace(bsnPesquisa.Login))
                {
                    colaboradoresDb = colaboradoresDb
                        .Where(x => x.Login.ToLower().Trim().RemoverAcentuacao().Contains(bsnPesquisa.Login.ToLower().Trim().RemoverAcentuacao()))
                        .ToList();
                }
                await _gravadorLogsBusiness.GravarMuitasVisualizacoesAsync(colaboradoresDb, BsnColunaLiterais.ListarIdsColunas("Colaborador", new [] { "Id", "Nome", "Login", "EstaAtivo" }, typeof(ColaboradorDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
                conteudoOk.AddRange(colaboradoresDb
                    .Select(colaboradorDb => new BsnRelacaoDeColaborador
                    {
                        Id = colaboradorDb.Id,
                        Nome = colaboradorDb.Nome,
                        Login = colaboradorDb.Login,
                        EstaAtivo = colaboradorDb.EstaAtivo
                    }));
            });
            if (!transacaoFoiOk)
            {
                return BsnResult<List<BsnRelacaoDeColaborador>>.Erro(Message.ErroNoServidor);
            }
        }
        else
        {
            var linqExpFiltro = new LinqExpModel<ColaboradorDbModel>();
            if (bsnPesquisa.EstaAtivo.HasValue)
            {
                linqExpFiltro.AppendAndAlso(x => x.EstaAtivo == bsnPesquisa.EstaAtivo.Value);
            }
            var colaboradoresDb = await _colaboradorRepository.SelectByLinqExpModelAsync(linqExpFiltro);
            if (!string.IsNullOrWhiteSpace(bsnPesquisa.Nome))
            {
                colaboradoresDb = colaboradoresDb
                    .Where(x => x.Nome.ToLower().Trim().RemoverAcentuacao().Contains(bsnPesquisa.Nome.ToLower().Trim().RemoverAcentuacao()))
                    .ToList();
            }
            if (!string.IsNullOrWhiteSpace(bsnPesquisa.Login))
            {
                colaboradoresDb = colaboradoresDb
                    .Where(x => x.Login.ToLower().Trim().RemoverAcentuacao().Contains(bsnPesquisa.Login.ToLower().Trim().RemoverAcentuacao()))
                    .ToList();
            }
            await _gravadorLogsBusiness.GravarMuitasVisualizacoesAsync(colaboradoresDb, BsnColunaLiterais.ListarIdsColunas("Colaborador", new [] { "Id", "Nome", "Login", "EstaAtivo" }, typeof(ColaboradorDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
            conteudoOk.AddRange(colaboradoresDb
                .Select(colaboradorDb => new BsnRelacaoDeColaborador
                {
                    Id = colaboradorDb.Id,
                    Nome = colaboradorDb.Nome,
                    Login = colaboradorDb.Login,
                    EstaAtivo = colaboradorDb.EstaAtivo
                }));
        }
        return BsnResult<List<BsnRelacaoDeColaborador>>.OkConteudo(conteudoOk);
    }

    public async Task<BsnResult<object>> AtivarAsync(string id)
    {
        var res = BsnResult<object>.Ok;
        var transacaoFoiOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var dbColaborador = await _colaboradorRepository.SelectByIdAsync(id);
            if (dbColaborador == null)
            {
                res = BsnResult<object>.Erro("Colaborador não encontrado.");
                return;
            }
            var resultOk = BsnResult<object>.Ok;
            resultOk.Mensagem = Message.RegistroAtivadoSucesso;
            if (dbColaborador.EstaAtivo)
            {
                return;
            }
            dbColaborador.EstaAtivo = true;
            await _colaboradorRepository.UpdateAsync(id, dbColaborador);
            await _gravadorLogsBusiness.GravarAlteracaoAsync(dbColaborador, BsnColunaLiterais.ListarIdsColunasSingle("Colaborador", "EstaAtivo", typeof(ColaboradorDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
        });
        if (!transacaoFoiOk)
        {
            return BsnResult<object>.Erro(Message.ErroNoServidor);
        }
        return res;
    }

    public async Task<BsnResult<object>> InativarAsync(string id)
    {
        var res = BsnResult<object>.Ok;
        var transacaoFoiOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var dbColaborador = await _colaboradorRepository.SelectByIdAsync(id);
            if (dbColaborador == null)
            {
                res = BsnResult<object>.Erro("Colaborador não encontrado.");
                return;
            }
            var resultOk = BsnResult<object>.Ok;
            resultOk.Mensagem = Message.RegistroInativadoSucesso;
            if (!dbColaborador.EstaAtivo)
            {
                return;
            }
            dbColaborador.EstaAtivo = false;
            await _colaboradorRepository.UpdateAsync(id, dbColaborador);
            await _gravadorLogsBusiness.GravarAlteracaoAsync(dbColaborador, BsnColunaLiterais.ListarIdsColunasSingle("Colaborador", "EstaAtivo", typeof(ColaboradorDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
        });
        if (!transacaoFoiOk)
        {
            return BsnResult<object>.Erro(Message.ErroNoServidor);
        }
        return res;
    }

    public async Task<BsnResult<BsnRelacaoDeColaborador>> ObterPorIdAsync(string id)
    {
        var uowSession = _dbSessionAccessor.DbSession;
        if (uowSession == null)
        {
            var res = BsnResult<BsnRelacaoDeColaborador>.Ok;
            var transacaoFoiOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
                var dbColaborador = await _colaboradorRepository.SelectByIdAsync(id);
                if (dbColaborador == null)
                {
                    res = BsnResult<BsnRelacaoDeColaborador>.Erro("Colaborador não encontrado.");
                    return;
                }
                await _gravadorLogsBusiness.GravarVisualizacaoAsync(dbColaborador, BsnColunaLiterais.ListarIdsColunas("Colaborador", new [] { "Id", "Nome", "Login", "EstaAtivo" }, typeof(ColaboradorDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
                res.Resultado = new BsnRelacaoDeColaborador
                {
                    Id = dbColaborador.Id,
                    Nome = dbColaborador.Nome,
                    Login = dbColaborador.Login,
                    EstaAtivo = dbColaborador.EstaAtivo
                };
            });
            if (!transacaoFoiOk)
            {
                return BsnResult<BsnRelacaoDeColaborador>.Erro(Message.ErroNoServidor);
            }
            return res;
        }
        else
        {
            var dbColaborador = await _colaboradorRepository.SelectByIdAsync(id);
            if (dbColaborador == null)
            {
                return BsnResult<BsnRelacaoDeColaborador>.Erro("Colaborador não encontrado.");
            }
            await _gravadorLogsBusiness.GravarVisualizacaoAsync(dbColaborador, BsnColunaLiterais.ListarIdsColunas("Colaborador", new [] { "Id", "Nome", "Login", "EstaAtivo" }, typeof(ColaboradorDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
            return BsnResult<BsnRelacaoDeColaborador>.OkConteudo(new BsnRelacaoDeColaborador
            {
                Id = dbColaborador.Id,
                Nome = dbColaborador.Nome,
                Login = dbColaborador.Login,
                EstaAtivo = dbColaborador.EstaAtivo
            });
        }
    }

    public async Task<BsnResult<List<BsnAcesso>>> ObterAcessosDoColaboradorAsync(string idColaborador)
    {
        var res = BsnResult<List<BsnAcesso>>.Ok;
        var transacaoFoiOk = await _uow.ExecuteTransactionAndReturnOkAsync(async () => {
            var dbColaborador = await _colaboradorRepository.SelectByIdAsync(idColaborador);
            if (dbColaborador == null)
            {
                res = BsnResult<List<BsnAcesso>>.Erro("Colaborador não encontrado.");
                return;
            }
            var acessosDb = await _acessoRepository.SelectByIdDoColaboradorAsync(idColaborador);
            await _gravadorLogsBusiness.GravarMuitasVisualizacoesAsync(acessosDb, BsnColunaLiterais.ListarIdsColunas("Acesso", new [] { "Id", "IdColaborador", "IdFuncionalidade", "EstaPermitido" }, typeof(AcessoDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
            res.Resultado = acessosDb
                .Select(acessoDb => new BsnAcesso
                {
                    IdFuncionalidade = acessoDb.IdFuncionalidade,
                    Funcionalidade = BsnFuncionalidadeLiterais.GetById(acessoDb.IdFuncionalidade),
                    EstaPermitido = acessoDb.EstaPermitido
                }).ToList();
        });
        if (!transacaoFoiOk)
        {
            return BsnResult<List<BsnAcesso>>.Erro(Message.ErroNoServidor);
        }
        return res;
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
                    var acessoInserido = new AcessoDbModel
                    {
                        Id = MongoId.NewMongoId,
                        IdColaborador = idColaborador,
                        IdFuncionalidade = iAcessoIn.IdFuncionalidade,
                        EstaPermitido = iAcessoIn.EstaPermitido
                    };
                    await _acessoRepository.InsertAsync(acessoInserido);
                    await _gravadorLogsBusiness.GravarInclusaoAsync(acessoInserido, _colaboradorLogadoAccessor.ColaboradorLogado.Id);
                    continue;
                }
                if (acessoDb.EstaPermitido != iAcessoIn.EstaPermitido)
                {
                    acessoDb.EstaPermitido = iAcessoIn.EstaPermitido;
                    await _acessoRepository.UpdateAsync(acessoDb.Id, acessoDb);
                    await _gravadorLogsBusiness.GravarAlteracaoAsync(acessoDb, BsnColunaLiterais.ListarIdsColunasSingle("Acesso", "EstaPermitido", typeof(AcessoDbModel)), _colaboradorLogadoAccessor.ColaboradorLogado.Id);
                }
            }
        });
        if (!transactionOk)
        {
            return BsnResult<object>.Erro(Message.ErroNoServidor);
        }
        return BsnResult<object>.OkMensagem(Message.RegistroAlteradoSucesso);
    }
}