namespace HoraH.Domain.Design.Patterns.Strategy.Md;
public class HrhMdItemDic<TValor>
{
    public int Indice { get; set; }
    public TValor ValorLogico { get; set; }
    public string Label { get; set; } = "";
}