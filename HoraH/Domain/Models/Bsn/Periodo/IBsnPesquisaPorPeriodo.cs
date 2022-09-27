namespace HoraH.Domain.Models.Bsn.Periodo;
public interface IBsnPesquisaPorPeriodo
{
    bool DateEValido(DateTime horaMarcada);
    BsnResult<object> ValidarRangesEObrigatorios();
}