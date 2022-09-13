using System.Text.Json;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.DbModels;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Business;
public class GravadorLogBusiness : IGravadorLogBusiness
{
    private readonly IRegistroRepository _registroRepository;
    private readonly IAlteracaoRegistroRepository _alteracaoRegistroRepository;
    private readonly IVisualizacaoRegistroRepository _visualizacaoRegistroRepository;
    private readonly IDadoRepository _dadoRepository;
    private readonly IUnitOfWork _uow;
    private readonly IDbSessionAccessor _dbSessionAccessor;
    public GravadorLogBusiness(IRegistroRepository registroRepository,
                               IAlteracaoRegistroRepository alteracaoRegistroRepository,
                               IVisualizacaoRegistroRepository visualizacaoRegistroRepository,
                               IDadoRepository dadoRepository,
                               IUnitOfWork uow,
                               IDbSessionAccessor dbSessionAccessor)
    {
        _registroRepository = registroRepository;
        _alteracaoRegistroRepository = alteracaoRegistroRepository;
        _visualizacaoRegistroRepository = visualizacaoRegistroRepository;
        _dadoRepository = dadoRepository;
        _uow = uow;
        _dbSessionAccessor = dbSessionAccessor;
    }

    private List<DadoDbModel> ExtrairDados<TDbModel>(TDbModel entidade)
    {
        var nomeColecao = typeof(TDbModel).Name.Split("DbModel")[0];
        var colunas = BsnColunaLiterais.ObterColunasDaDbModel(nomeColecao, typeof(TDbModel));
        var dados = new List<DadoDbModel>();
        foreach (var iColuna in colunas)
        {
            var propertyInfo = typeof(TDbModel).GetProperty(iColuna.NomeColuna);
            dados.Add(new DadoDbModel
            {
                Id = MongoId.NewMongoId,
                IdColuna = iColuna.Id,
                ValorSerializadoJson = JsonSerializer.Serialize(propertyInfo.GetValue(entidade), propertyInfo.PropertyType)
            });
        }
        return dados;
    }

    public async Task<string> GravarInclusaoAsync<TDbModel>(TDbModel entidade, string idColaboradorInclusao)
    {
        var uowSession = _dbSessionAccessor.DbSession;
        if (uowSession == null)
        {
            await _uow.StartTransactionAsync();
            try
            {
                var hrInclusao = DateTime.Now;
                var registroInserido = new RegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorInclusao = idColaboradorInclusao,
                    HoraInclusao = hrInclusao,
                    FoiExcluido = false,
                    EstaEsperandoAprovacao = false
                };
                var dadosInclusao = ExtrairDados(entidade);
                foreach (var iDadoInclusao in dadosInclusao)
                {
                    iDadoInclusao.IdTipoRegistro = BsnTipoRegistroLiterais.Inclusao.Id;
                    iDadoInclusao.IdRegistroGenerico = registroInserido.Id;
                }
                await _registroRepository.InsertAsync(registroInserido);
                await _dadoRepository.InsertManyAsync(dadosInclusao);
                await _uow.CommitTransactionAsync();
                return registroInserido.Id;
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }
        else
        {
            var hrInclusao = DateTime.Now;
            var registroInserido = new RegistroDbModel
            {
                Id = MongoId.NewMongoId,
                IdColaboradorInclusao = idColaboradorInclusao,
                HoraInclusao = hrInclusao,
                FoiExcluido = false,
                EstaEsperandoAprovacao = false
            };
            var dadosInclusao = ExtrairDados(entidade);
            foreach (var iDadoInclusao in dadosInclusao)
            {
                iDadoInclusao.IdTipoRegistro = BsnTipoRegistroLiterais.Inclusao.Id;
                iDadoInclusao.IdRegistroGenerico = registroInserido.Id;
            }
            await _registroRepository.InsertAsync(registroInserido);
            await _dadoRepository.InsertManyAsync(dadosInclusao);
            return registroInserido.Id;
        }
    }

    public async Task<string> GravarAlteracaoAsync<TDbModel>(TDbModel entidadeAposSalvar, List<string> idsColunasAlteracao, string idColaboradorAlteracao)
    {
        var uowSession = _dbSessionAccessor.DbSession;
        var nomeColecao = typeof(TDbModel).Name.Split("DbModel")[0];
        if (uowSession == null)
        {
            await _uow.StartTransactionAsync();
            try
            {
                var hrAlteracao = DateTime.Now;
                var dadosEntidade = ExtrairDados(entidadeAposSalvar);
                var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var entidadeIdSerializadoJson = dadosEntidade.First(x => x.IdColuna == colunaEId.Id).ValorSerializadoJson;
                var linqExpDadoInclusaoId = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJson
                    && x.IdColuna == colunaEId.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadoInclusaoId = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoId)).First();
                var registroAlterado = await _registroRepository.SelectByIdAsync(dadoInclusaoId.IdRegistroGenerico);
                var alteracaoRegistroInserido = new AlteracaoRegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorAlteracao = idColaboradorAlteracao,
                    HoraAlteracao = hrAlteracao,
                    EstaEsperandoAprovacao = false
                };
                var dadosEntidadeFiltrado = dadosEntidade.Where(x => idsColunasAlteracao.Contains(x.IdColuna));
                foreach (var iDadoEntidade in dadosEntidadeFiltrado)
                {
                    iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Alteracao.Id;
                    iDadoEntidade.IdRegistroGenerico = alteracaoRegistroInserido.Id;
                }
                await _alteracaoRegistroRepository.InsertAsync(alteracaoRegistroInserido);
                await _dadoRepository.InsertManyAsync(dadosEntidadeFiltrado);
                await _uow.CommitTransactionAsync();
                return alteracaoRegistroInserido.Id;
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }
        else
        {
            var hrAlteracao = DateTime.Now;
            var dadosEntidade = ExtrairDados(entidadeAposSalvar);
            var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
            var entidadeIdSerializadoJson = dadosEntidade.First(x => x.IdColuna == colunaEId.Id).ValorSerializadoJson;
            var linqExpDadoInclusaoId = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJson
                && x.IdColuna == colunaEId.Id
                && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadoInclusaoId = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoId)).First();
            var registroAlterado = await _registroRepository.SelectByIdAsync(dadoInclusaoId.IdRegistroGenerico);
            var alteracaoRegistroInserido = new AlteracaoRegistroDbModel
            {
                Id = MongoId.NewMongoId,
                IdColaboradorAlteracao = idColaboradorAlteracao,
                HoraAlteracao = hrAlteracao,
                EstaEsperandoAprovacao = false
            };
            var dadosEntidadeFiltrado = dadosEntidade.Where(x => idsColunasAlteracao.Contains(x.IdColuna));
            foreach (var iDadoEntidade in dadosEntidadeFiltrado)
            {
                iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Alteracao.Id;
                iDadoEntidade.IdRegistroGenerico = alteracaoRegistroInserido.Id;
            }
            await _alteracaoRegistroRepository.InsertAsync(alteracaoRegistroInserido);
            await _dadoRepository.InsertManyAsync(dadosEntidadeFiltrado);
            return alteracaoRegistroInserido.Id;
        }
    }

    public async Task<string> GravarAlteracaoAutoDiffsAsync<TDbModel>(TDbModel entidadeAntesSalvar, TDbModel entidadeAposSalvar, string idColaboradorAlteracao)
    {
        var uowSession = _dbSessionAccessor.DbSession;
        var nomeColecao = typeof(TDbModel).Name.Split("DbModel")[0];
        if (uowSession == null)
        {
            await _uow.StartTransactionAsync();
            try
            {
                var hrAlteracao = DateTime.Now;
                var dadosEntidade = ExtrairDados(entidadeAposSalvar);
                var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var entidadeIdSerializadoJson = dadosEntidade.First(x => x.IdColuna == colunaEId.Id).ValorSerializadoJson;
                var linqExpDadoInclusaoId = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJson
                    && x.IdColuna == colunaEId.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadoInclusaoId = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoId)).First();
                var registroAlterado = await _registroRepository.SelectByIdAsync(dadoInclusaoId.IdRegistroGenerico);
                var alteracaoRegistroInserido = new AlteracaoRegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorAlteracao = idColaboradorAlteracao,
                    HoraAlteracao = hrAlteracao,
                    EstaEsperandoAprovacao = false
                };
                var dadosEntidadeAntesComparacao = ExtrairDados(entidadeAntesSalvar);
                var idsColunasAlteracao = dadosEntidade.Where(x => dadosEntidadeAntesComparacao.Any(y => y.IdColuna == x.IdColuna && y.ValorSerializadoJson == x.ValorSerializadoJson)).Select(x => x.IdColuna).ToList();
                var dadosEntidadeFiltrado = dadosEntidade.Where(x => idsColunasAlteracao.Contains(x.IdColuna));
                foreach (var iDadoEntidade in dadosEntidadeFiltrado)
                {
                    iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Alteracao.Id;
                    iDadoEntidade.IdRegistroGenerico = alteracaoRegistroInserido.Id;
                }
                await _alteracaoRegistroRepository.InsertAsync(alteracaoRegistroInserido);
                await _dadoRepository.InsertManyAsync(dadosEntidadeFiltrado);
                await _uow.CommitTransactionAsync();
                return alteracaoRegistroInserido.Id;
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }
        else
        {
            var hrAlteracao = DateTime.Now;
            var dadosEntidade = ExtrairDados(entidadeAposSalvar);
            var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
            var entidadeIdSerializadoJson = dadosEntidade.First(x => x.IdColuna == colunaEId.Id).ValorSerializadoJson;
            var linqExpDadoInclusaoId = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJson
                && x.IdColuna == colunaEId.Id
                && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadoInclusaoId = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoId)).First();
            var registroAlterado = await _registroRepository.SelectByIdAsync(dadoInclusaoId.IdRegistroGenerico);
            var alteracaoRegistroInserido = new AlteracaoRegistroDbModel
            {
                Id = MongoId.NewMongoId,
                IdColaboradorAlteracao = idColaboradorAlteracao,
                HoraAlteracao = hrAlteracao,
                EstaEsperandoAprovacao = false
            };
            var dadosEntidadeAntesComparacao = ExtrairDados(entidadeAntesSalvar);
            var idsColunasAlteracao = dadosEntidade.Where(x => dadosEntidadeAntesComparacao.Any(y => y.IdColuna == x.IdColuna && y.ValorSerializadoJson == x.ValorSerializadoJson)).Select(x => x.IdColuna).ToList();
            var dadosEntidadeFiltrado = dadosEntidade.Where(x => idsColunasAlteracao.Contains(x.IdColuna));
            foreach (var iDadoEntidade in dadosEntidadeFiltrado)
            {
                iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Alteracao.Id;
                iDadoEntidade.IdRegistroGenerico = alteracaoRegistroInserido.Id;
            }
            await _alteracaoRegistroRepository.InsertAsync(alteracaoRegistroInserido);
            await _dadoRepository.InsertManyAsync(dadosEntidadeFiltrado);
            return alteracaoRegistroInserido.Id;
        }
    }

    public async Task<string> GravarVisualizacaoAsync<TDbModel>(TDbModel entidadeVisualizada, List<string> idsColunasVisualizacao, string idColaboradorVisualizacao)
    {
        var uowSession = _dbSessionAccessor.DbSession;
        var nomeColecao = typeof(TDbModel).Name.Split("DbModel")[0];
        if (uowSession == null)
        {
            await _uow.StartTransactionAsync();
            try
            {
                var hrVisualizacao = DateTime.Now;
                var dadosEntidade = ExtrairDados(entidadeVisualizada);
                var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var entidadeIdSerializadoJson = dadosEntidade.First(x => x.IdColuna == colunaEId.Id).ValorSerializadoJson;
                var linqExpDadoInclusaoId = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJson
                    && x.IdColuna == colunaEId.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadoInclusaoId = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoId)).First();
                var registroVisualizado = await _registroRepository.SelectByIdAsync(dadoInclusaoId.IdRegistroGenerico);
                var visualizacaoRegistroInserido = new VisualizacaoRegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorVisualizacao = idColaboradorVisualizacao,
                    HoraVisualizacao = hrVisualizacao,
                };
                var dadosEntidadeFiltrado = dadosEntidade.Where(x => idsColunasVisualizacao.Contains(x.IdColuna));
                foreach (var iDadoEntidade in dadosEntidadeFiltrado)
                {
                    iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Visualizacao.Id;
                    iDadoEntidade.IdRegistroGenerico = visualizacaoRegistroInserido.Id;
                }
                await _visualizacaoRegistroRepository.InsertAsync(visualizacaoRegistroInserido);
                await _dadoRepository.InsertManyAsync(dadosEntidadeFiltrado);
                await _uow.CommitTransactionAsync();
                return visualizacaoRegistroInserido.Id;
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }
        else
        {
            var hrVisualizacao = DateTime.Now;
            var dadosEntidade = ExtrairDados(entidadeVisualizada);
            var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
            var entidadeIdSerializadoJson = dadosEntidade.First(x => x.IdColuna == colunaEId.Id).ValorSerializadoJson;
            var linqExpDadoInclusaoId = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJson
                && x.IdColuna == colunaEId.Id
                && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadoInclusaoId = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoId)).First();
            var registroVisualizado = await _registroRepository.SelectByIdAsync(dadoInclusaoId.IdRegistroGenerico);
            var visualizacaoRegistroInserido = new VisualizacaoRegistroDbModel
            {
                Id = MongoId.NewMongoId,
                IdColaboradorVisualizacao = idColaboradorVisualizacao,
                HoraVisualizacao = hrVisualizacao,
            };
            var dadosEntidadeFiltrado = dadosEntidade.Where(x => idsColunasVisualizacao.Contains(x.IdColuna));
            foreach (var iDadoEntidade in dadosEntidadeFiltrado)
            {
                iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Visualizacao.Id;
                iDadoEntidade.IdRegistroGenerico = visualizacaoRegistroInserido.Id;
            }
            await _visualizacaoRegistroRepository.InsertAsync(visualizacaoRegistroInserido);
            await _dadoRepository.InsertManyAsync(dadosEntidadeFiltrado);
            return visualizacaoRegistroInserido.Id;
        }
    }

    public Task<string> GravarMuitasVisualizacoesAsync<TDbModel>(List<TDbModel> entidadesVisualizadas, List<string> idsColunasVisualizacao, string idColaboradorVisualizacao)
    {
        throw new NotImplementedException();
    }

    public Task<string> GravarExclusaoAsync(string nomeTabela, string idEntidade, string idColaboradorExclusao)
    {
        throw new NotImplementedException();
    }
}