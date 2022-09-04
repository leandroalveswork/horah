namespace HoraH.Domain.Models;
public static class Uteis
{
    public static string ListarPortugues(this List<string> itens)
    {
        var idxItem = itens.Count - 1;
        var texto = "";
        while (idxItem > -1)
        {
            if (idxItem == itens.Count - 1)
            {
                texto = itens[idxItem];
            }
            else if (idxItem == itens.Count - 2)
            {
                texto = $"{itens[idxItem]} e {texto}";
            }
            else
            {
                texto = $"{itens[idxItem]}, {texto}";
            }
            idxItem--;
        }
        return texto;
    }

    public static string ExtrairQueryParamDeUrl(this string url, string key)
    {
        var divisaoPorInterrog = url.Split('?');
        if (divisaoPorInterrog.Length < 2) {
            return "";
        }
        var paramsDaUrl = divisaoPorInterrog[1];
        var divisaoPorEComercial = paramsDaUrl.Split('&');
        var paramComAKey = divisaoPorEComercial.FirstOrDefault(x => x.Contains(key + "="));
        if (paramComAKey == null) {
            return "";
        }
        var divisaoPorIgual = paramComAKey.Split('=');
        if (divisaoPorIgual.Length < 2) {
            return "";
        }
        var vlParam = divisaoPorIgual[1];
        return vlParam;
    }
}