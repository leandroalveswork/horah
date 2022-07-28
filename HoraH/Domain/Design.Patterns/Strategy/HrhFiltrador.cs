namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhFiltrador
{
    public static IEnumerable<TItem> FiltrarPeloPredicate<TItem, TFiltro>(IEnumerable<TItem> itens, Func<TItem, bool> predicate, TFiltro filtro)
    {
        if (filtro == null)
        {
            return itens;
        }
        return itens.Where(predicate);
    }
    public static IEnumerable<T> FiltrarPeloTexto<T>(IEnumerable<T> itens, Func<T, string> predicateDoMembro, string? filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro))
        {
            return itens;
        }
        return itens.Where(x => predicateDoMembro(x).Contains(filtro));
    }
}