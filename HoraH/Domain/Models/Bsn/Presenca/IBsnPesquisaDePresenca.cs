using HoraH.Domain.Models.LinqExp;

namespace HoraH.Domain.Models.Bsn.Presenca;
public interface IBsnPesquisaDePresenca
{
    string? NomeColaborador { get; set; }
    string? IdEvento { get; set; }
    int? MinutosTrabalhadosMinimo { get; set; }
    int? MinutosTrabalhadosMaximo { get; set; }
    bool FoiInformadoIntervaloMinutosTrabalhados { get; }
    BsnResult<object> ValidarRangesEObrigatorios();
    bool DateEValido(DateTime horaMarcada);
}