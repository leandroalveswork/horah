using HoraH.Domain.Models.Bsn.Presenca;

namespace HoraH.Domain.Models.ViewModels;
public class RelacaoDoDiaTrabViewModel
{
    public int MinutosTrabalhados { get; set; }
    public DateTime Dia { get; set; }
    public List<BsnRelacaoDePresenca> PresencasDoDia { get; set; } = new List<BsnRelacaoDePresenca>();
    public bool TemPresencasExpandido { get; set; }
    public static RelacaoDoDiaTrabViewModel FromBsnRelacao(BsnRelacaoDoDiaTrabalhado bsnRelacao)
    {
        return new RelacaoDoDiaTrabViewModel
        {
            MinutosTrabalhados = bsnRelacao.MinutosTrabalhados,
            Dia = bsnRelacao.Dia,
            PresencasDoDia = new List<BsnRelacaoDePresenca>(bsnRelacao.PresencasDoDia),
            TemPresencasExpandido = false
        };
    }
}