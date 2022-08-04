namespace HoraH.Domain.Design.Patterns.Strategy.Md;
public class HrhMdItemDicSuporte : HrhMdItemDic<HrhMdValorItemDicSuporte>
{
    public override int CompararLogicoCom(HrhMdValorItemDicSuporte outroLogico)
    {
        return ValorLogico.Id.CompareTo(outroLogico.Id);
    }
}