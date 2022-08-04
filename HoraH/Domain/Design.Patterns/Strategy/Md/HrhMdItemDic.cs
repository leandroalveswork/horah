namespace HoraH.Domain.Design.Patterns.Strategy.Md;
public abstract class HrhMdItemDic<TValor>
{
    public int Indice { get; set; }
    public TValor ValorLogico { get; set; }
    public string Label { get; set; } = "";
    public abstract int CompararLogicoCom(TValor outroLogico);
}