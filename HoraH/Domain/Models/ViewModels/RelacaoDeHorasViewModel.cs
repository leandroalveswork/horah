using HoraH.Domain.Models.Bsn.Presenca;

namespace HoraH.Domain.Models.ViewModels;
public class RelacaoDeHorasViewModel
{
    public string IdColaborador { get; set; } = "";
    public string NomeColaborador { get; set; } = "";
    public List<RelacaoDoDiaTrabViewModel> DiasTrabalhados { get; set; } = new List<RelacaoDoDiaTrabViewModel>();
    public bool TemDiasExpandido { get; set; }
    public static RelacaoDeHorasViewModel FromBsnRelacao(BsnRelacaoDeHorasDoColaborador bsnRelacao)
    {
        return new RelacaoDeHorasViewModel
        {
            IdColaborador = bsnRelacao.IdColaborador,
            NomeColaborador = bsnRelacao.NomeColaborador,
            DiasTrabalhados = bsnRelacao.DiasTrabalhados.Select(x => RelacaoDoDiaTrabViewModel.FromBsnRelacao(x)).ToList(),
            TemDiasExpandido = true
        };
    }
}