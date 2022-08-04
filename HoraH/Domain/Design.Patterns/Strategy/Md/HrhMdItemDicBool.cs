namespace HoraH.Domain.Design.Patterns.Strategy.Md;
public class HrhMdItemDicBool : HrhMdItemDic<bool?>
{
    private int GetValorInterno(bool? vl)
    {
        if (vl == null)
        {
            return 0;
        }
        if (!vl.Value)
        {
            return 1;
        }
        return 2;
    }
    
    public override int CompararLogicoCom(bool? outroLogico)
    {
        return GetValorInterno(ValorLogico) - GetValorInterno(outroLogico);
    }
}