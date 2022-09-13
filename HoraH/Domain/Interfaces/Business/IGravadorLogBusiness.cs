namespace HoraH.Domain.Interfaces.Business;
public interface IGravadorLogBusiness
{
    Task<string> GravarInclusaoAsync<TDbModel>(TDbModel entidade, string idColaboradorInclusao);
    Task<string> GravarAlteracaoAsync<TDbModel>(TDbModel entidadeAposSalvar, List<string> idsColunasAlteracao, string idColaboradorAlteracao);
    Task<string> GravarAlteracaoAutoDiffsAsync<TDbModel>(TDbModel entidadeAntesSalvar, TDbModel entidadeAposSalvar, string idColaboradorAlteracao);
    Task<string> GravarVisualizacaoAsync<TDbModel>(TDbModel entidadeVisualizada, List<string> idsColunasVisualizacao, string idColaboradorVisualizacao);
    Task<string> GravarMuitasVisualizacoesAsync<TDbModel>(List<TDbModel> entidadesVisualizadas, List<string> idsColunasVisualizacao, string idColaboradorVisualizacao);
    Task<string> GravarExclusaoAsync(string nomeTabela, string idEntidade, string idColaboradorExclusao);
}