using System.Linq.Expressions;
using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.Configuration;
using HoraH.Domain.Interfaces.Repository;
using HoraH.Domain.Models.Bsn;
using HoraH.Domain.Models.Bsn.Autorizacao;
using HoraH.Domain.Models.DbModels;
using HoraH.Repository.Common;
using MongoDB.Driver;

namespace HoraH.Repository;
public class FuncionalidadeRepository : RepositoryBase<FuncionalidadeDbModel>, IFuncionalidadeRepository
{
    public FuncionalidadeRepository(IDbClientAccessor dbClientAccessor, IAppConfiguration appConfiguration, IDbSessionAccessor dbSessionAccessor)
        : base(dbClientAccessor, appConfiguration, dbSessionAccessor)
    {
    }

    protected override string GetNomeColecEntidade()
    {
        return _appConfiguration.NomeColecFuncionalidade;
    }

    protected override Expression<Func<FuncionalidadeDbModel, bool>> GetMatchesId(string id)
    {
        return x => x.Id == id;
    }

    public List<FuncionalidadeDbModel> ListarFuncionalidadesDoSistema()
    {
        return new List<FuncionalidadeDbModel>()
        {
            new FuncionalidadeDbModel() { Id = "d7986a067dd1462184a52b95", Nome = BsnFuncionalidadeDoSistema.ColaboradorListar },
            new FuncionalidadeDbModel() { Id = "07a9c0011b694b6ea0f313e8", Nome = BsnFuncionalidadeDoSistema.ColaboradorAcessoAlterar },
            new FuncionalidadeDbModel() { Id = "4a4d8f2e96624c0c9f07478c", Nome = BsnFuncionalidadeDoSistema.ColaboradorAtivar },
            new FuncionalidadeDbModel() { Id = "73eecd812a62432ab882cf07", Nome = BsnFuncionalidadeDoSistema.RegistroListar },
            new FuncionalidadeDbModel() { Id = "74d08b30a38844a4a05df475", Nome = BsnFuncionalidadeDoSistema.PresencaMarcar },
            new FuncionalidadeDbModel() { Id = "07106a4cd5e043f1b148468f", Nome = BsnFuncionalidadeDoSistema.PresencaListar },
            new FuncionalidadeDbModel() { Id = "94cb52635fa9444b8f51446c", Nome = BsnFuncionalidadeDoSistema.SolicitacaoIncluir },
            new FuncionalidadeDbModel() { Id = "88bded7eff82408fbc29655d", Nome = BsnFuncionalidadeDoSistema.SolicitacaoListar },
            new FuncionalidadeDbModel() { Id = "ec1214d7b9c348aebaaf07c0", Nome = BsnFuncionalidadeDoSistema.SolicitacaoAprovar }
        };
    }

    public string GetNomeDaFuncionalidadeComId(string? id)
    {
        var funcionalidade = ListarFuncionalidadesDoSistema().FirstOrDefault(x => x.Id == id);
        if (funcionalidade == null)
        {
            return "";
        }
        return funcionalidade.Nome;
    }

    public string GetIdDaFuncionalidadeComNome(string nome)
    {
        var funcionalidade = ListarFuncionalidadesDoSistema().FirstOrDefault(x => x.Nome == nome);
        if (funcionalidade == null)
        {
            return "";
        }
        return funcionalidade.Id;
    }
}