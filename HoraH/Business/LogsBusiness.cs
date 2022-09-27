using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Colaborador;
using HoraH.Domain.Models.Bsn.Logs;
using HoraH.Domain.Models.Bsn.Presenca;
using HoraH.Domain.Models.DbModels;
using HoraH.Domain.Models.LinqExp;

namespace HoraH.Business;
public class LogsBusiness : ILogsBusiness
{
    private readonly IColaboradorBusiness _colaboradorBusiness;
    private readonly IRegistroRepository _registroRepository;
    private readonly IAlteracaoRegistroRepository _alteracaoRegistroRepository;
    private readonly IVisualizacaoRegistroRepository _visualizacaoRegistroRepository;
    private readonly IDadoRepository _dadoRepository;
    public LogsBusiness(IColaboradorBusiness colaboradorBusiness,
                        IRegistroRepository registroRepository,
                        IAlteracaoRegistroRepository alteracaoRegistroRepository,
                        IVisualizacaoRegistroRepository visualizacaoRegistroRepository,
                        IDadoRepository dadoRepository)
    {
        _colaboradorBusiness = colaboradorBusiness;
        _registroRepository = registroRepository;
        _alteracaoRegistroRepository = alteracaoRegistroRepository;
        _visualizacaoRegistroRepository = visualizacaoRegistroRepository;
        _dadoRepository = dadoRepository;
    }

    public async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarAsync(BsnPesquisaDeLogs pesquisa, TimeZoneInfo timeZone)
    {
        var resValid = pesquisa.Periodo.ValidarRangesEObrigatorios();
        if (!resValid.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeLog>>.Erro(resValid.Mensagem);
        }
        if (string.IsNullOrEmpty(pesquisa.IdOperacao))
        {
            var lConteudoRes = new List<BsnRelacaoDeLog>();
            var resOpInclusao = await PesquisarOpInclusaoAsync(pesquisa, timeZone);
            if (!resOpInclusao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpInclusao.Mensagem);
            }
            lConteudoRes.AddRange(resOpInclusao.Resultado);
            var resOpAlteracao = await PesquisarOpAlteracaoAsync(pesquisa, timeZone);
            if (!resOpAlteracao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpAlteracao.Mensagem);
            }
            lConteudoRes.AddRange(resOpAlteracao.Resultado);
            var resOpVisualizacao = await PesquisarOpVisualizacaoAsync(pesquisa, timeZone);
            if (!resOpVisualizacao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpVisualizacao.Mensagem);
            }
            lConteudoRes.AddRange(resOpVisualizacao.Resultado);
            var resOpExclusao = await PesquisarOpExclusaoAsync(pesquisa, timeZone);
            if (!resOpExclusao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpExclusao.Mensagem);
            }
            lConteudoRes.AddRange(resOpExclusao.Resultado);
            return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(lConteudoRes);
        }
        if (pesquisa.IdOperacao == BsnOperacaoLiterais.Inclusao.Id)
        {
            var resOpInclusao = await PesquisarOpInclusaoAsync(pesquisa, timeZone);
            if (!resOpInclusao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpInclusao.Mensagem);
            }
            return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(resOpInclusao.Resultado);
        }
        if (pesquisa.IdOperacao == BsnOperacaoLiterais.Alteracao.Id)
        {
            var resOpAlteracao = await PesquisarOpAlteracaoAsync(pesquisa, timeZone);
            if (!resOpAlteracao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpAlteracao.Mensagem);
            }
            return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(resOpAlteracao.Resultado);
        }
        if (pesquisa.IdOperacao == BsnOperacaoLiterais.Visualizacao.Id)
        {
            var resOpVisualizacao = await PesquisarOpVisualizacaoAsync(pesquisa, timeZone);
            if (!resOpVisualizacao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpVisualizacao.Mensagem);
            }
            return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(resOpVisualizacao.Resultado);
        }
        if (pesquisa.IdOperacao == BsnOperacaoLiterais.Exclusao.Id)
        {
            var resOpExclusao = await PesquisarOpExclusaoAsync(pesquisa, timeZone);
            if (!resOpExclusao.EstaOk)
            {
                return BsnResult<List<BsnRelacaoDeLog>>.Erro(resOpExclusao.Mensagem);
            }
            return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(resOpExclusao.Resultado);
        }
        return BsnResult<List<BsnRelacaoDeLog>>.Erro("Não foi possível fazer a pesquisa.");
    }

    private async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarOpInclusaoAsync(BsnPesquisaDeLogs pesquisa, TimeZoneInfo timeZone)
    {
        var linqExpFiltro = new LinqExpModel<RegistroDbModel>();
        var filtroColaboradores = new BsnPesquisaDeColaborador { Nome = pesquisa.NomeColaborador, EstaAtivo = true };
        var resColaboradoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroColaboradores);
        if (!resColaboradoresRelac.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeLog>>.Erro(resColaboradoresRelac.Mensagem);
        }
        if (!string.IsNullOrWhiteSpace(pesquisa.NomeColaborador))
        {
            var idsColaboradores = resColaboradoresRelac.Resultado.Select(x => x.Id);
            linqExpFiltro.AppendAndAlso(x => idsColaboradores.Contains(x.IdColaboradorInclusao));
        }
        if (!string.IsNullOrEmpty(pesquisa.IdTabela))
        {
            var nomeTabela = BsnTabelaLiterais.GetById(pesquisa.IdTabela).Nome;
            var idsColunasTabela = BsnColunaLiterais.ListarTodos().Where(x => x.NomeTabela == nomeTabela).Select(x => x.Id);
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id && idsColunasTabela.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosCujosDadosSaoDaTabela = dadosDb.Select(x => x.IdRegistroGenerico).Distinct();
            linqExpFiltro.AppendAndAlso(x => idsRegistrosCujosDadosSaoDaTabela.Contains(x.Id));
        }
        var idsColunasSaoId = BsnColunaLiterais.ObterColunasSaoId();
        if (!string.IsNullOrEmpty(pesquisa.IdEntidade))
        {
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id && idsColunasSaoId.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosComId = new List<string>();
            foreach (var dadoDb in dadosDb)
            {
                if (dadoDb.ValorSerializadoJson.AsDeserializadoJson<string>() == pesquisa.IdEntidade)
                {
                    idsRegistrosComId.Add(dadoDb.IdRegistroGenerico);
                }
            }
            linqExpFiltro.AppendAndAlso(x => idsRegistrosComId.Contains(x.Id));
        }
        // if (pesquisa.DataOperacaoInicio.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => BsnDateTimeModel.FromDb(x.HoraInclusao, timeZone).Value >= pesquisa.DataOperacaoInicio.Value.Date);
        // }
        // if (pesquisa.DataOperacaoFim.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => BsnDateTimeModel.FromDb(x.HoraInclusao, timeZone).Value < pesquisa.DataOperacaoFim.Value.Date.AddDays(1));
        // }
        var registrosDb = (await _registroRepository.SelectByLinqExpModelAsync(linqExpFiltro))
            .Where(x => pesquisa.Periodo.DateEValido(BsnDateTimeModel.FromDb(x.HoraInclusao, timeZone).Value));

        var idsRegistros = registrosDb.Select(x => x.Id);
        var linqExpDadosDosRegistros = new LinqExpModel<DadoDbModel>(x => idsRegistros.Contains(x.IdRegistroGenerico) && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
        var dadosDosRegistrosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDosRegistros);
        var conteudoOk = new List<BsnRelacaoDeLog>();
        foreach (var iRegistroDb in registrosDb)
        {
            var iConteudo = new BsnRelacaoDeLog
            {
                Id = iRegistroDb.Id,
                NomeColaboradorOperador = resColaboradoresRelac.Resultado.First(x => x.Id == iRegistroDb.IdColaboradorInclusao).Nome,
                IdOperacao = BsnOperacaoLiterais.Inclusao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(iRegistroDb.HoraInclusao, timeZone).Value 
            };
            var dadosDoRegistro = dadosDosRegistrosDb.Where(x => x.IdRegistroGenerico == iRegistroDb.Id);
            var eCompleto = false;
            foreach (var iDadoDoRegistro in dadosDoRegistro)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    iConteudo.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    iConteudo.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                    eCompleto = true;
                    break;
                }
            }
            if (eCompleto)
            {
                conteudoOk.Add(iConteudo);
            }
        }
        return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(conteudoOk);
    }
    
    private async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarOpAlteracaoAsync(BsnPesquisaDeLogs pesquisa, TimeZoneInfo timeZone)
    {
        var linqExpFiltro = new LinqExpModel<AlteracaoRegistroDbModel>();
        var filtroColaboradores = new BsnPesquisaDeColaborador { Nome = pesquisa.NomeColaborador, EstaAtivo = true };
        var resColaboradoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroColaboradores);
        if (!resColaboradoresRelac.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeLog>>.Erro(resColaboradoresRelac.Mensagem);
        }
        if (!string.IsNullOrWhiteSpace(pesquisa.NomeColaborador))
        {
            var idsColaboradores = resColaboradoresRelac.Resultado.Select(x => x.Id);
            linqExpFiltro.AppendAndAlso(x => idsColaboradores.Contains(x.IdColaboradorAlteracao));
        }
        if (!string.IsNullOrEmpty(pesquisa.IdTabela))
        {
            var nomeTabela = BsnTabelaLiterais.GetById(pesquisa.IdTabela).Nome;
            var idsColunasTabela = BsnColunaLiterais.ListarTodos().Where(x => x.NomeTabela == nomeTabela).Select(x => x.Id);
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Alteracao.Id && idsColunasTabela.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosCujosDadosSaoDaTabela = dadosDb.Select(x => x.IdRegistroGenerico).Distinct();
            linqExpFiltro.AppendAndAlso(x => idsRegistrosCujosDadosSaoDaTabela.Contains(x.Id));
        }
        var idsColunasSaoId = BsnColunaLiterais.ObterColunasSaoId();
        if (!string.IsNullOrEmpty(pesquisa.IdEntidade))
        {
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id && idsColunasSaoId.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosComId = new List<string>();
            foreach (var dadoDb in dadosDb)
            {
                if (dadoDb.ValorSerializadoJson.AsDeserializadoJson<string>() == pesquisa.IdEntidade)
                {
                    idsRegistrosComId.Add(dadoDb.IdRegistroGenerico);
                }
            }
            linqExpFiltro.AppendAndAlso(x => idsRegistrosComId.Contains(x.IdRegistroAlterado));
        }
        // if (pesquisa.DataOperacaoInicio.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => BsnDateTimeModel.FromDb(x.HoraAlteracao, timeZone).Value >= pesquisa.DataOperacaoInicio.Value.Date);
        // }
        // if (pesquisa.DataOperacaoFim.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => BsnDateTimeModel.FromDb(x.HoraAlteracao, timeZone).Value < pesquisa.DataOperacaoFim.Value.Date.AddDays(1));
        // }
        var alteracoesRegistrosDb = (await _alteracaoRegistroRepository.SelectByLinqExpModelAsync(linqExpFiltro))
            .Where(x => pesquisa.Periodo.DateEValido(BsnDateTimeModel.FromDb(x.HoraAlteracao, timeZone).Value));
        var idsRegistros = alteracoesRegistrosDb.Select(x => x.IdRegistroAlterado).Distinct();
        var linqExpDadosDosRegistros = new LinqExpModel<DadoDbModel>(x => idsRegistros.Contains(x.IdRegistroGenerico) && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
        var dadosDosRegistrosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDosRegistros);
        var conteudoOk = new List<BsnRelacaoDeLog>();
        foreach (var iAlteracaoRegistroDb in alteracoesRegistrosDb)
        {
            var iConteudo = new BsnRelacaoDeLog
            {
                Id = iAlteracaoRegistroDb.Id,
                NomeColaboradorOperador = resColaboradoresRelac.Resultado.First(x => x.Id == iAlteracaoRegistroDb.IdColaboradorAlteracao).Nome,
                IdOperacao = BsnOperacaoLiterais.Alteracao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(iAlteracaoRegistroDb.HoraAlteracao, timeZone).Value 
            };
            var dadosDoRegistro = dadosDosRegistrosDb.Where(x => x.IdRegistroGenerico == iAlteracaoRegistroDb.IdRegistroAlterado);
            var eCompleto = false;
            foreach (var iDadoDoRegistro in dadosDoRegistro)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    iConteudo.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    iConteudo.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                    eCompleto = true;
                    break;
                }
            }
            if (eCompleto)
            {
                conteudoOk.Add(iConteudo);
            }
        }
        return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(conteudoOk);
    }

    private async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarOpVisualizacaoAsync(BsnPesquisaDeLogs pesquisa, TimeZoneInfo timeZone)
    {
        var linqExpFiltro = new LinqExpModel<VisualizacaoRegistroDbModel>();
        var filtroColaboradores = new BsnPesquisaDeColaborador { Nome = pesquisa.NomeColaborador, EstaAtivo = true };
        var resColaboradoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroColaboradores);
        if (!resColaboradoresRelac.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeLog>>.Erro(resColaboradoresRelac.Mensagem);
        }
        if (!string.IsNullOrWhiteSpace(pesquisa.NomeColaborador))
        {
            var idsColaboradores = resColaboradoresRelac.Resultado.Select(x => x.Id);
            linqExpFiltro.AppendAndAlso(x => idsColaboradores.Contains(x.IdColaboradorVisualizacao));
        }
        if (!string.IsNullOrEmpty(pesquisa.IdTabela))
        {
            var nomeTabela = BsnTabelaLiterais.GetById(pesquisa.IdTabela).Nome;
            var idsColunasTabela = BsnColunaLiterais.ListarTodos().Where(x => x.NomeTabela == nomeTabela).Select(x => x.Id);
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Visualizacao.Id && idsColunasTabela.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosCujosDadosSaoDaTabela = dadosDb.Select(x => x.IdRegistroGenerico).Distinct();
            linqExpFiltro.AppendAndAlso(x => idsRegistrosCujosDadosSaoDaTabela.Contains(x.Id));
        }
        var idsColunasSaoId = BsnColunaLiterais.ObterColunasSaoId();
        if (!string.IsNullOrEmpty(pesquisa.IdEntidade))
        {
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id && idsColunasSaoId.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosComId = new List<string>();
            foreach (var dadoDb in dadosDb)
            {
                if (dadoDb.ValorSerializadoJson.AsDeserializadoJson<string>() == pesquisa.IdEntidade)
                {
                    idsRegistrosComId.Add(dadoDb.IdRegistroGenerico);
                }
            }
            linqExpFiltro.AppendAndAlso(x => idsRegistrosComId.Contains(x.IdRegistroVisualizado));
        }
        // if (pesquisa.DataOperacaoInicio.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => BsnDateTimeModel.FromDb(x.HoraVisualizacao, timeZone).Value >= pesquisa.DataOperacaoInicio.Value.Date);
        // }
        // if (pesquisa.DataOperacaoFim.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => BsnDateTimeModel.FromDb(x.HoraVisualizacao, timeZone).Value < pesquisa.DataOperacaoFim.Value.Date.AddDays(1));
        // }
        var visualizacoesRegistrosDb = (await _visualizacaoRegistroRepository.SelectByLinqExpModelAsync(linqExpFiltro))
            .Where(x => pesquisa.Periodo.DateEValido(BsnDateTimeModel.FromDb(x.HoraVisualizacao, timeZone).Value));
        var idsRegistros = visualizacoesRegistrosDb.Select(x => x.IdRegistroVisualizado).Distinct();
        var linqExpDadosDosRegistros = new LinqExpModel<DadoDbModel>(x => idsRegistros.Contains(x.IdRegistroGenerico) && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
        var dadosDosRegistrosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDosRegistros);
        var conteudoOk = new List<BsnRelacaoDeLog>();
        foreach (var iVisualizacaoRegistro in visualizacoesRegistrosDb)
        {
            var iConteudo = new BsnRelacaoDeLog
            {
                Id = iVisualizacaoRegistro.Id,
                NomeColaboradorOperador = resColaboradoresRelac.Resultado.First(x => x.Id == iVisualizacaoRegistro.IdColaboradorVisualizacao).Nome,
                IdOperacao = BsnOperacaoLiterais.Visualizacao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(iVisualizacaoRegistro.HoraVisualizacao, timeZone).Value 
            };
            var dadosDoRegistro = dadosDosRegistrosDb.Where(x => x.IdRegistroGenerico == iVisualizacaoRegistro.IdRegistroVisualizado);
            var eCompleto = false;
            foreach (var iDadoDoRegistro in dadosDoRegistro)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    iConteudo.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    iConteudo.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                    eCompleto = true;
                    break;
                }
            }
            if (eCompleto)
            {
                conteudoOk.Add(iConteudo);
            }
        }
        return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(conteudoOk);
    }

    private async Task<BsnResult<List<BsnRelacaoDeLog>>> PesquisarOpExclusaoAsync(BsnPesquisaDeLogs pesquisa, TimeZoneInfo timeZone)
    {
        var linqExpFiltro = new LinqExpModel<RegistroDbModel>(x => x.FoiExcluido);
        var filtroColaboradores = new BsnPesquisaDeColaborador { Nome = pesquisa.NomeColaborador, EstaAtivo = true };
        var resColaboradoresRelac = await _colaboradorBusiness.PesquisarAsync(filtroColaboradores);
        if (!resColaboradoresRelac.EstaOk)
        {
            return BsnResult<List<BsnRelacaoDeLog>>.Erro(resColaboradoresRelac.Mensagem);
        }
        if (!string.IsNullOrWhiteSpace(pesquisa.NomeColaborador))
        {
            var idsColaboradores = resColaboradoresRelac.Resultado.Select(x => x.Id);
            linqExpFiltro.AppendAndAlso(x => idsColaboradores.Contains(x.IdColaboradorExclusao));
        }
        if (!string.IsNullOrEmpty(pesquisa.IdTabela))
        {
            var nomeTabela = BsnTabelaLiterais.GetById(pesquisa.IdTabela).Nome;
            var idsColunasTabela = BsnColunaLiterais.ListarTodos().Where(x => x.NomeTabela == nomeTabela).Select(x => x.Id);
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id && idsColunasTabela.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosCujosDadosSaoDaTabela = dadosDb.Select(x => x.IdRegistroGenerico).Distinct();
            linqExpFiltro.AppendAndAlso(x => idsRegistrosCujosDadosSaoDaTabela.Contains(x.Id));
        }
        var idsColunasSaoId = BsnColunaLiterais.ObterColunasSaoId();
        if (!string.IsNullOrEmpty(pesquisa.IdEntidade))
        {
            var linqExpDados = new LinqExpModel<DadoDbModel>(x => x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id && idsColunasSaoId.Contains(x.IdColuna));
            var dadosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDados);
            var idsRegistrosComId = new List<string>();
            foreach (var dadoDb in dadosDb)
            {
                if (dadoDb.ValorSerializadoJson.AsDeserializadoJson<string>() == pesquisa.IdEntidade)
                {
                    idsRegistrosComId.Add(dadoDb.IdRegistroGenerico);
                }
            }
            linqExpFiltro.AppendAndAlso(x => idsRegistrosComId.Contains(x.Id));
        }
        // if (pesquisa.DataOperacaoInicio.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => x.HoraExclusao.HasValue && BsnDateTimeModel.FromDb(x.HoraExclusao.Value, timeZone).Value >= pesquisa.DataOperacaoInicio.Value.Date);
        // }
        // if (pesquisa.DataOperacaoFim.HasValue)
        // {
        //     linqExpFiltro.AppendAndAlso(x => x.HoraExclusao.HasValue && BsnDateTimeModel.FromDb(x.HoraExclusao.Value, timeZone).Value < pesquisa.DataOperacaoFim.Value.Date.AddDays(1));
        // }
        var registrosDb = (await _registroRepository.SelectByLinqExpModelAsync(linqExpFiltro))
            .Where(x => x.HoraExclusao.HasValue && pesquisa.Periodo.DateEValido(BsnDateTimeModel.FromDb(x.HoraExclusao.Value, timeZone).Value));
        var idsRegistros = registrosDb.Select(x => x.Id);
        var linqExpDadosDosRegistros = new LinqExpModel<DadoDbModel>(x => idsRegistros.Contains(x.IdRegistroGenerico) && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
        var dadosDosRegistrosDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDosRegistros);
        var conteudoOk = new List<BsnRelacaoDeLog>();
        foreach (var iRegistroDb in registrosDb)
        {
            var iConteudo = new BsnRelacaoDeLog
            {
                Id = iRegistroDb.Id,
                NomeColaboradorOperador = resColaboradoresRelac.Resultado.First(x => x.Id == iRegistroDb.IdColaboradorExclusao).Nome,
                IdOperacao = BsnOperacaoLiterais.Exclusao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(iRegistroDb.HoraExclusao.Value, timeZone).Value
            };
            var dadosDoRegistro = dadosDosRegistrosDb.Where(x => x.IdRegistroGenerico == iRegistroDb.Id);
            var eCompleto = false;
            foreach (var iDadoDoRegistro in dadosDoRegistro)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    iConteudo.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    iConteudo.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                    eCompleto = true;
                    break;
                }
            }
            if (eCompleto)
            {
                conteudoOk.Add(iConteudo);
            }
        }
        return BsnResult<List<BsnRelacaoDeLog>>.OkConteudo(conteudoOk);
    }

    public async Task<BsnResult<BsnRelacaoDeLog>> ObterPorIdAsync(string idOperacao, string idRegistro, TimeZoneInfo timeZone)
    {
        var idsColunasSaoId = BsnColunaLiterais.ObterColunasSaoId();
        if (idOperacao == BsnOperacaoLiterais.Inclusao.Id)
        {
            var linqExpFiltro = new LinqExpModel<RegistroDbModel>(x => x.Id == idRegistro);
            var registrosDb = await _registroRepository.SelectByLinqExpModelAsync(linqExpFiltro);
            if (registrosDb == null || !registrosDb.Any())
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            var registroDb = registrosDb.First();
            var linqExpDadosDoRegistroDb = new LinqExpModel<DadoDbModel>(x => x.IdRegistroGenerico == idRegistro && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadosDoRegistroDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDoRegistroDb);
            var conteudoOk = new BsnRelacaoDeLog
            {
                Id = idRegistro,
                IdOperacao = BsnOperacaoLiterais.Inclusao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(registroDb.HoraInclusao, timeZone).Value 
            };
            var resColaboradorPorId = await _colaboradorBusiness.ObterPorIdAsync(registroDb.IdColaboradorInclusao);
            if (!resColaboradorPorId.EstaOk)
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            conteudoOk.NomeColaboradorOperador = resColaboradorPorId.Resultado.Nome;
            foreach (var iDadoDoRegistro in dadosDoRegistroDb)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    conteudoOk.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    conteudoOk.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                }
                conteudoOk.Dados.Add(new BsnRelacaoDeDado
                {
                    Id = iDadoDoRegistro.Id,
                    IdColuna = iDadoDoRegistro.IdColuna,
                    ValorSerializadoJson = iDadoDoRegistro.ValorSerializadoJson
                });
            }
            return BsnResult<BsnRelacaoDeLog>.OkConteudo(conteudoOk);
        }
        if (idOperacao == BsnOperacaoLiterais.Alteracao.Id)
        {
            var linqExpFiltro = new LinqExpModel<AlteracaoRegistroDbModel>(x => x.Id == idRegistro);
            var alteracoesRegistrosDb = await _alteracaoRegistroRepository.SelectByLinqExpModelAsync(linqExpFiltro);
            if (alteracoesRegistrosDb == null || !alteracoesRegistrosDb.Any())
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            var alteracaoRegistroDb = alteracoesRegistrosDb.First();
            var linqExpDadosDoRegistroDb = new LinqExpModel<DadoDbModel>(x => x.IdRegistroGenerico == alteracaoRegistroDb.IdRegistroAlterado && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadosDoRegistroDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDoRegistroDb);
            var linqExpDadosDaAlteracaoRegistro = new LinqExpModel<DadoDbModel>(x => x.IdRegistroGenerico == idRegistro && x.IdTipoRegistro == BsnTipoRegistroLiterais.Alteracao.Id);
            var dadosDaAlteracaoRegistroDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDaAlteracaoRegistro);
            var conteudoOk = new BsnRelacaoDeLog
            {
                Id = idRegistro,
                IdOperacao = BsnOperacaoLiterais.Alteracao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(alteracaoRegistroDb.HoraAlteracao, timeZone).Value 
            };
            var resColaboradorPorId = await _colaboradorBusiness.ObterPorIdAsync(alteracaoRegistroDb.IdColaboradorAlteracao);
            if (!resColaboradorPorId.EstaOk)
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            conteudoOk.NomeColaboradorOperador = resColaboradorPorId.Resultado.Nome;
            foreach (var iDadoDoRegistro in dadosDoRegistroDb)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    conteudoOk.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    conteudoOk.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                    break;
                }
            }
            foreach (var iDadoDoRegistro in dadosDaAlteracaoRegistroDb)
            {
                conteudoOk.Dados.Add(new BsnRelacaoDeDado
                {
                    Id = iDadoDoRegistro.Id,
                    IdColuna = iDadoDoRegistro.IdColuna,
                    ValorSerializadoJson = iDadoDoRegistro.ValorSerializadoJson
                });
            }
            return BsnResult<BsnRelacaoDeLog>.OkConteudo(conteudoOk);
        }
        if (idOperacao == BsnOperacaoLiterais.Visualizacao.Id)
        {
            var visualizacaoRegistroDb = await _visualizacaoRegistroRepository.SelectByIdAsync(idRegistro);
            if (visualizacaoRegistroDb == null)
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            var linqExpDadosDoRegistroDb = new LinqExpModel<DadoDbModel>(x => x.IdRegistroGenerico == visualizacaoRegistroDb.IdRegistroVisualizado && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadosDoRegistroDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDoRegistroDb);
            var linqExpDadosDaVisualizacaoRegistro = new LinqExpModel<DadoDbModel>(x => x.IdRegistroGenerico == idRegistro && x.IdTipoRegistro == BsnTipoRegistroLiterais.Visualizacao.Id);
            var dadosDaVisualizacaoRegistroDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDaVisualizacaoRegistro);
            var conteudoOk = new BsnRelacaoDeLog
            {
                Id = idRegistro,
                IdOperacao = BsnOperacaoLiterais.Visualizacao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(visualizacaoRegistroDb.HoraVisualizacao, timeZone).Value 
            };
            var resColaboradorPorId = await _colaboradorBusiness.ObterPorIdAsync(visualizacaoRegistroDb.IdColaboradorVisualizacao);
            if (!resColaboradorPorId.EstaOk)
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            conteudoOk.NomeColaboradorOperador = resColaboradorPorId.Resultado.Nome;
            foreach (var iDadoDoRegistro in dadosDoRegistroDb)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    conteudoOk.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    conteudoOk.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                    break;
                }
            }
            foreach (var iDadoDoRegistro in dadosDaVisualizacaoRegistroDb)
            {
                conteudoOk.Dados.Add(new BsnRelacaoDeDado
                {
                    Id = iDadoDoRegistro.Id,
                    IdColuna = iDadoDoRegistro.IdColuna,
                    ValorSerializadoJson = iDadoDoRegistro.ValorSerializadoJson
                });
            }
            return BsnResult<BsnRelacaoDeLog>.OkConteudo(conteudoOk);
        }
        if (idOperacao == BsnOperacaoLiterais.Exclusao.Id)
        {
            var linqExpFiltro = new LinqExpModel<RegistroDbModel>(x => x.FoiExcluido && x.Id == idRegistro);
            var registrosDb = await _registroRepository.SelectByLinqExpModelAsync(linqExpFiltro);
            if (registrosDb == null || !registrosDb.Any())
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            var registroDb = registrosDb.First();
            var linqExpDadosDoRegistroDb = new LinqExpModel<DadoDbModel>(x => x.IdRegistroGenerico == idRegistro && x.IdTipoRegistro == BsnTipoRegistroLiterais.Inclusao.Id);
            var dadosDoRegistroDb = await _dadoRepository.SelectByLinqExpModelAsync(linqExpDadosDoRegistroDb);
            var conteudoOk = new BsnRelacaoDeLog
            {
                Id = idRegistro,
                IdOperacao = BsnOperacaoLiterais.Exclusao.Id,
                HoraOperacao = BsnDateTimeModel.FromDb(registroDb.HoraExclusao.Value, timeZone).Value 
            };
            var resColaboradorPorId = await _colaboradorBusiness.ObterPorIdAsync(registroDb.IdColaboradorExclusao);
            if (!resColaboradorPorId.EstaOk)
            {
                return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
            }
            conteudoOk.NomeColaboradorOperador = resColaboradorPorId.Resultado.Nome;
            foreach (var iDadoDoRegistro in dadosDoRegistroDb)
            {
                if (idsColunasSaoId.Contains(iDadoDoRegistro.IdColuna))
                {
                    conteudoOk.IdEntidade = iDadoDoRegistro.ValorSerializadoJson.AsDeserializadoJson<string>();
                    conteudoOk.IdTabela = BsnTabelaLiterais.GetByNome(BsnColunaLiterais.GetById(iDadoDoRegistro.IdColuna).NomeTabela).Id;
                }
                conteudoOk.Dados.Add(new BsnRelacaoDeDado
                {
                    Id = iDadoDoRegistro.Id,
                    IdColuna = iDadoDoRegistro.IdColuna,
                    ValorSerializadoJson = iDadoDoRegistro.ValorSerializadoJson
                });
            }
            return BsnResult<BsnRelacaoDeLog>.OkConteudo(conteudoOk);
        }
        return BsnResult<BsnRelacaoDeLog>.Erro("Registro não encontrado");
    }
}