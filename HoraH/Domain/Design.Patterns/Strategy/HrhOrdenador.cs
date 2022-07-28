namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhOrdenador
{
    public static IEnumerable<TItem> OrdenarBaseadoNoNumeroDaColuna<TItem>(IEnumerable<TItem> itens, int numeroDaColuna, bool ehCrescente, params Func<TItem, object>[] predicateDasColunas)
    {
        if (numeroDaColuna >= predicateDasColunas.Length || numeroDaColuna <= -1)
        {
            return itens;
        }
        return (ehCrescente) ?
            (itens.OrderBy(predicateDasColunas[numeroDaColuna])) :
            (itens.OrderByDescending(predicateDasColunas[numeroDaColuna]));
    }
}