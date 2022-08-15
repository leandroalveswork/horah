using HoraH.Domain.Interfaces.Business;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Models.Bsn.Funcionalidade;

namespace HoraH.Business;
public class FuncionalidadeBusiness : IFuncionalidadeBusiness
{
    public List<BsnFuncionalidade> ListarFuncionalidadesDoSistema()
    {
        return new List<BsnFuncionalidade>()
        {
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.ListarColaboradorId, Nome = BsnFuncionalidadeLiterais.ListarColaborador },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.AlterarAcessoId, Nome = BsnFuncionalidadeLiterais.AlterarAcesso },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.AtivarColaboradorId, Nome = BsnFuncionalidadeLiterais.AtivarColaborador },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.ListarLogId, Nome = BsnFuncionalidadeLiterais.ListarLog },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.MarcarPresencaId, Nome = BsnFuncionalidadeLiterais.MarcarPresenca },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.ListarPresencaId, Nome = BsnFuncionalidadeLiterais.ListarPresenca },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.IncluirSolicitacaoId, Nome = BsnFuncionalidadeLiterais.IncluirSolicitacao },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.ListarSolicitacaoId, Nome = BsnFuncionalidadeLiterais.ListarSolicitacao },
            new BsnFuncionalidade { Id = BsnFuncionalidadeLiterais.AprovarSolicitacaoId, Nome = BsnFuncionalidadeLiterais.AprovarSolicitacao }
        };
    }

    public BsnFuncionalidade? GetFuncionalidadePorId(string id)
    {
        return ListarFuncionalidadesDoSistema().FirstOrDefault(x => x.Id == id);
    }
}