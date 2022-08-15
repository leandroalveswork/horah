using HoraH.Domain.Models.Bsn.Funcionalidade;
using HoraH.Domain.Models.DbModels;

namespace HoraH.Domain.Interfaces.Business;
public interface IFuncionalidadeBusiness
{
    List<BsnFuncionalidade> ListarFuncionalidadesDoSistema();
    BsnFuncionalidade? GetFuncionalidadePorId(string id);
}