using HoraH.Domain.Models.Bsn.Presenca;

namespace HoraH.Domain.Models.ViewModels;
public class RelacaoDoDiaTrabViewModel
{
    public string IdColaborador { get; set; } = "";
    public string NomeColaborador { get; set; } = "";
    public int MinutosTrabalhados { get; set; }
    public DateTime Dia { get; set; }
    public string HorasView => MinutosTrabalhados.AsTotalHorasFormatted();
    public string DiaView => Dia.ToString("dd/MM/yyyy");
    public static RelacaoDoDiaTrabViewModel FromBsnRelacao(BsnRelacaoDoDiaTrabalhado bsnRelacao)
    {
        return new RelacaoDoDiaTrabViewModel
        {
            IdColaborador = bsnRelacao.IdColaborador,
            NomeColaborador = bsnRelacao.NomeColaborador,
            MinutosTrabalhados = bsnRelacao.MinutosTrabalhados,
            Dia = bsnRelacao.Dia
        };
    }
}