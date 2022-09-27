using System.Text.Json;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Proxy;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.DbModels;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Business;
public class GravadorLogBusiness : IGravadorLogBusiness
{
    private readonly IColunaLiteralProxy _colunaLiteralProxy;
    private readonly IRegistroRepository _registroRepository;
    private readonly IAlteracaoRegistroRepository _alteracaoRegistroRepository;
    private readonly IVisualizacaoRegistroRepository _visualizacaoRegistroRepository;
    private readonly IDadoRepository _dadoRepository;
    private readonly IUnitOfWork _uow;
    private readonly IDbSessionAccessor _dbSessionAccessor;
    public GravadorLogBusiness(IColunaLiteralProxy colunaLiteralProxy,
                               IRegistroRepository registroRepository,
                               IAlteracaoRegistroRepository alteracaoRegistroRepository,
                               IVisualizacaoRegistroRepository visualizacaoRegistroRepository,
                               IDadoRepository dadoRepository,
                               IUnitOfWork uow,
                               IDbSessionAccessor dbSessionAccessor)
    {
        _colunaLiteralProxy = colunaLiteralProxy;
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
                var hrInclusaoInUow = DateTime.Now;
                var registroInseridoInUow = new RegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorInclusao = idColaboradorInclusao,
                    HoraInclusao = hrInclusaoInUow,
                    FoiExcluido = false,
                };
                var dadosInclusaoInUow = ExtrairDados(entidade);
                foreach (var iDadoInclusaoInUow in dadosInclusaoInUow)
                {
                    iDadoInclusaoInUow.IdTipoRegistro = BsnTipoRegistroLiterais.Inclusao.Id;
                    iDadoInclusaoInUow.IdRegistroGenerico = registroInseridoInUow.Id;
                }
                await _registroRepository.InsertAsync(registroInseridoInUow);
                await _dadoRepository.InsertManyAsync(dadosInclusaoInUow);
                await _uow.CommitTransactionAsync();
                return registroInseridoInUow.Id;
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
                var hrAlteracaoInUow = DateTime.Now;
                var dadosEntidadeInUow = ExtrairDados(entidadeAposSalvar);
                var colunaEIdInUow = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var entidadeIdSerializadoJsonInUow = dadosEntidadeInUow.First(x => x.IdColuna == colunaEIdInUow.Id).ValorSerializadoJson;
                var linqExpDadoInclusaoIdInUow = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJsonInUow
                    && x.IdColuna == colunaEIdInUow.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadoInclusaoIdInUow = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoIdInUow)).First();
                var alteracaoRegistroInseridoInUow = new AlteracaoRegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorAlteracao = idColaboradorAlteracao,
                    IdRegistroAlterado = dadoInclusaoIdInUow.IdRegistroGenerico,
                    HoraAlteracao = hrAlteracaoInUow,
                };
                var dadosEntidadeFiltradoInUow = dadosEntidadeInUow.Where(x => idsColunasAlteracao.Contains(x.IdColuna));
                foreach (var iDadoEntidadeInUow in dadosEntidadeFiltradoInUow)
                {
                    iDadoEntidadeInUow.IdTipoRegistro = BsnTipoRegistroLiterais.Alteracao.Id;
                    iDadoEntidadeInUow.IdRegistroGenerico = alteracaoRegistroInseridoInUow.Id;
                }
                await _alteracaoRegistroRepository.InsertAsync(alteracaoRegistroInseridoInUow);
                await _dadoRepository.InsertManyAsync(dadosEntidadeFiltradoInUow);
                await _uow.CommitTransactionAsync();
                return alteracaoRegistroInseridoInUow.Id;
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
            var alteracaoRegistroInserido = new AlteracaoRegistroDbModel
            {
                Id = MongoId.NewMongoId,
                IdColaboradorAlteracao = idColaboradorAlteracao,
                IdRegistroAlterado = dadoInclusaoId.IdRegistroGenerico,
                HoraAlteracao = hrAlteracao,
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
                var hrAlteracaoInUow = DateTime.Now;
                var dadosEntidadeInUow = ExtrairDados(entidadeAposSalvar);
                var colunaEIdInUow = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var entidadeIdSerializadoJsonInUow = dadosEntidadeInUow.First(x => x.IdColuna == colunaEIdInUow.Id).ValorSerializadoJson;
                var linqExpDadoInclusaoIdInUow = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJsonInUow
                    && x.IdColuna == colunaEIdInUow.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadoInclusaoIdInUow = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoIdInUow)).First();
                var alteracaoRegistroInseridoInUow = new AlteracaoRegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorAlteracao = idColaboradorAlteracao,
                    IdRegistroAlterado = dadoInclusaoIdInUow.IdRegistroGenerico,
                    HoraAlteracao = hrAlteracaoInUow,
                };
                var dadosEntidadeAntesComparacaoInUow = ExtrairDados(entidadeAntesSalvar);
                var idsColunasAlteracaoInUow = dadosEntidadeInUow.Where(x => dadosEntidadeAntesComparacaoInUow.Any(y => y.IdColuna == x.IdColuna && y.ValorSerializadoJson == x.ValorSerializadoJson)).Select(x => x.IdColuna).ToList();
                var dadosEntidadeFiltradoInUow = dadosEntidadeInUow.Where(x => idsColunasAlteracaoInUow.Contains(x.IdColuna));
                foreach (var iDadoEntidadeInUow in dadosEntidadeFiltradoInUow)
                {
                    iDadoEntidadeInUow.IdTipoRegistro = BsnTipoRegistroLiterais.Alteracao.Id;
                    iDadoEntidadeInUow.IdRegistroGenerico = alteracaoRegistroInseridoInUow.Id;
                }
                await _alteracaoRegistroRepository.InsertAsync(alteracaoRegistroInseridoInUow);
                await _dadoRepository.InsertManyAsync(dadosEntidadeFiltradoInUow);
                await _uow.CommitTransactionAsync();
                return alteracaoRegistroInseridoInUow.Id;
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
            var alteracaoRegistroInserido = new AlteracaoRegistroDbModel
            {
                Id = MongoId.NewMongoId,
                IdColaboradorAlteracao = idColaboradorAlteracao,
                IdRegistroAlterado = dadoInclusaoId.IdRegistroGenerico,
                HoraAlteracao = hrAlteracao,
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
                var hrVisualizacaoInUow = DateTime.Now;
                var dadosEntidadeInUow = ExtrairDados(entidadeVisualizada);
                var colunaEIdInUow = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var entidadeIdSerializadoJsonInUow = dadosEntidadeInUow.First(x => x.IdColuna == colunaEIdInUow.Id).ValorSerializadoJson;
                var linqExpDadoInclusaoIdInUow = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJsonInUow
                    && x.IdColuna == colunaEIdInUow.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadoInclusaoIdInUow = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoIdInUow)).First();
                var visualizacaoRegistroInseridoInUow = new VisualizacaoRegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorVisualizacao = idColaboradorVisualizacao,
                    IdRegistroVisualizado = dadoInclusaoIdInUow.IdRegistroGenerico,
                    HoraVisualizacao = hrVisualizacaoInUow,
                };
                var dadosEntidadeFiltradoInUow = dadosEntidadeInUow.Where(x => idsColunasVisualizacao.Contains(x.IdColuna));
                foreach (var iDadoEntidadeInUow in dadosEntidadeFiltradoInUow)
                {
                    iDadoEntidadeInUow.IdTipoRegistro = BsnTipoRegistroLiterais.Visualizacao.Id;
                    iDadoEntidadeInUow.IdRegistroGenerico = visualizacaoRegistroInseridoInUow.Id;
                }
                await _visualizacaoRegistroRepository.InsertAsync(visualizacaoRegistroInseridoInUow);
                await _dadoRepository.InsertManyAsync(dadosEntidadeFiltradoInUow);
                await _uow.CommitTransactionAsync();
                return visualizacaoRegistroInseridoInUow.Id;
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
            var visualizacaoRegistroInserido = new VisualizacaoRegistroDbModel
            {
                Id = MongoId.NewMongoId,
                IdColaboradorVisualizacao = idColaboradorVisualizacao,
                IdRegistroVisualizado = dadoInclusaoId.IdRegistroGenerico,
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

    public async Task<List<string>> GravarMuitasVisualizacoesAsync<TDbModel>(List<TDbModel> entidadesVisualizadas, List<string> idsColunasVisualizacao, string idColaboradorVisualizacao)
    {
        var uowSession = _dbSessionAccessor.DbSession;
        var nomeColecao = typeof(TDbModel).Name.Split("DbModel")[0];
        if (uowSession == null)
        {
            await _uow.StartTransactionAsync();
            try
            {
                var hrVisualizacaoInUow = DateTime.Now;
                var visualizacoesGravadasInUow = new List<BsnVisualizacaoGravada>();
                foreach (var iEntidadeVisInUow in entidadesVisualizadas)
                {
                    visualizacoesGravadasInUow.Add(new BsnVisualizacaoGravada { DadosExtraidos = ExtrairDados(iEntidadeVisInUow) });
                }
                var colunaEIdInUow = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var idsSerializadosEntidadesInUow = visualizacoesGravadasInUow.Select(x => x.ObterIdSerializadoJson(colunaEIdInUow)).ToList();
                var linqExpDadosInclusaoIdInUow = new LinqExpModel<DadoDbModel>(x => idsSerializadosEntidadesInUow.Contains(x.ValorSerializadoJson)
                    && x.IdColuna == colunaEIdInUow.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadosInclusaoIdInUow = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosInclusaoIdInUow);
                foreach (var iVisGravadaInUow in visualizacoesGravadasInUow)
                {
                    iVisGravadaInUow.DadoEIdDb = dadosInclusaoIdInUow.First(x => x.ValorSerializadoJson == iVisGravadaInUow.ObterIdSerializadoJson(colunaEIdInUow));
                }
                var insercaoManyVisualizacoesInUow = new List<VisualizacaoRegistroDbModel>();
                var insercaoManyDadosInUow = new List<DadoDbModel>();
                var retornoIdsInUow = new List<string>();
                foreach (var iVisGravadaInUow in visualizacoesGravadasInUow)
                {
                    var visualizacaoRegistroInseridoInUow = new VisualizacaoRegistroDbModel
                    {
                        Id = MongoId.NewMongoId,
                        IdColaboradorVisualizacao = idColaboradorVisualizacao,
                        IdRegistroVisualizado = iVisGravadaInUow.DadoEIdDb.IdRegistroGenerico,
                        HoraVisualizacao = hrVisualizacaoInUow,
                    };
                    insercaoManyVisualizacoesInUow.Add(visualizacaoRegistroInseridoInUow);
                    var dadosEntidadeFiltrado = iVisGravadaInUow.DadosExtraidos.Where(x => idsColunasVisualizacao.Contains(x.IdColuna));
                    foreach (var iDadoEntidade in dadosEntidadeFiltrado)
                    {
                        iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Visualizacao.Id;
                        iDadoEntidade.IdRegistroGenerico = visualizacaoRegistroInseridoInUow.Id;
                    }
                    insercaoManyDadosInUow.AddRange(dadosEntidadeFiltrado);
                    retornoIdsInUow.Add(visualizacaoRegistroInseridoInUow.Id);
                }
                await _visualizacaoRegistroRepository.InsertManyAsync(insercaoManyVisualizacoesInUow);
                await _dadoRepository.InsertManyAsync(insercaoManyDadosInUow);
                await _uow.CommitTransactionAsync();
                return retornoIdsInUow;
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
            var visualizacoesGravadas = new List<BsnVisualizacaoGravada>();
            foreach (var iEntidadeVis in entidadesVisualizadas)
            {
                visualizacoesGravadas.Add(new BsnVisualizacaoGravada { DadosExtraidos = ExtrairDados(iEntidadeVis) });
            }
            var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
            var idsSerializadosEntidades = visualizacoesGravadas.Select(x => x.ObterIdSerializadoJson(colunaEId)).ToList();
            var linqExpDadosInclusaoId = new LinqExpModel<DadoDbModel>(x => idsSerializadosEntidades.Contains(x.ValorSerializadoJson)
                && x.IdColuna == colunaEId.Id
                && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadosInclusaoId = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosInclusaoId);
            foreach (var iVisGravada in visualizacoesGravadas)
            {
                iVisGravada.DadoEIdDb = dadosInclusaoId.First(x => x.ValorSerializadoJson == iVisGravada.ObterIdSerializadoJson(colunaEId));
            }
            var insercaoManyVisualizacoes = new List<VisualizacaoRegistroDbModel>();
            var insercaoManyDados = new List<DadoDbModel>();
            var retornoIds = new List<string>();
            foreach (var iVisGravada in visualizacoesGravadas)
            {
                var visualizacaoRegistroInserido = new VisualizacaoRegistroDbModel
                {
                    Id = MongoId.NewMongoId,
                    IdColaboradorVisualizacao = idColaboradorVisualizacao,
                    IdRegistroVisualizado = iVisGravada.DadoEIdDb.IdRegistroGenerico,
                    HoraVisualizacao = hrVisualizacao,
                };
                insercaoManyVisualizacoes.Add(visualizacaoRegistroInserido);
                var dadosEntidadeFiltrado = iVisGravada.DadosExtraidos.Where(x => idsColunasVisualizacao.Contains(x.IdColuna));
                foreach (var iDadoEntidade in dadosEntidadeFiltrado)
                {
                    iDadoEntidade.IdTipoRegistro = BsnTipoRegistroLiterais.Visualizacao.Id;
                    iDadoEntidade.IdRegistroGenerico = visualizacaoRegistroInserido.Id;
                }
                insercaoManyDados.AddRange(dadosEntidadeFiltrado);
                retornoIds.Add(visualizacaoRegistroInserido.Id);
            }
            await _visualizacaoRegistroRepository.InsertManyAsync(insercaoManyVisualizacoes);
            await _dadoRepository.InsertManyAsync(insercaoManyDados);
            return retornoIds;
        }
    }

    public async Task GravarExclusaoAsync<TDbModel>(string idEntidade, string idColaboradorExclusao)
    {
        var uowSession = _dbSessionAccessor.DbSession;
        var nomeColecao = typeof(TDbModel).Name.Split("DbModel")[0];
        if (uowSession == null)
        {
            await _uow.StartTransactionAsync();
            try
            {
                var hrExclusaoInUow = DateTime.Now;
                var colunaEIdInUow = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
                var entidadeIdSerializadoJsonInUow = idEntidade.AsSerializadoJson();
                var linqExpDadoInclusaoIdInUow = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJsonInUow
                    && x.IdColuna == colunaEIdInUow.Id
                    && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
                var dadoInclusaoIdInUow = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoIdInUow)).First();
                var registroExcluidoDbInUow = await _registroRepository.SelectByIdAsync(dadoInclusaoIdInUow.IdRegistroGenerico);
                registroExcluidoDbInUow.FoiExcluido = true;
                registroExcluidoDbInUow.IdColaboradorExclusao = idColaboradorExclusao;
                registroExcluidoDbInUow.HoraExclusao = hrExclusaoInUow;
                await _registroRepository.UpdateAsync(registroExcluidoDbInUow.Id, registroExcluidoDbInUow);
                await _uow.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }
        else
        {   
            var hrExclusao = DateTime.Now;
            var colunaEId = BsnColunaLiterais.ObterColunaEspecificaPorNome(nomeColecao, "Id", typeof(TDbModel));
            var entidadeIdSerializadoJson = idEntidade.AsSerializadoJson();
            var linqExpDadoInclusaoId = new LinqExpModel<DadoDbModel>(x => x.ValorSerializadoJson == entidadeIdSerializadoJson
                && x.IdColuna == colunaEId.Id
                && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadoInclusaoId = (await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadoInclusaoId)).First();
            var registroExcluidoDb = await _registroRepository.SelectByIdAsync(dadoInclusaoId.IdRegistroGenerico);
            registroExcluidoDb.FoiExcluido = true;
            registroExcluidoDb.IdColaboradorExclusao = idColaboradorExclusao;
            registroExcluidoDb.HoraExclusao = hrExclusao;
            await _registroRepository.UpdateAsync(registroExcluidoDb.Id, registroExcluidoDb);
        }
    }
}