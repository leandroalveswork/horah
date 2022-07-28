namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhPaginador
{
    public static IEnumerable<TItem> Paginar<TItem>(IEnumerable<TItem> itens, int numeroDaPagina, int resultadosPorPagina, out int numeroDaPaginaCorrigido)
    {
        numeroDaPaginaCorrigido = numeroDaPagina;
        if (numeroDaPaginaCorrigido < 1)
        {
            numeroDaPaginaCorrigido = 1;
        }
        var totalPaginas = Convert.ToInt32(Math.Ceiling(((double)itens.Count()) / resultadosPorPagina));
        if (totalPaginas == 0)
        {
            totalPaginas = 1;
        }
        if (numeroDaPaginaCorrigido > totalPaginas)
        {
            numeroDaPaginaCorrigido = totalPaginas;
        }
        return itens.Skip((numeroDaPaginaCorrigido - 1) * resultadosPorPagina).Take(resultadosPorPagina);
    }
}