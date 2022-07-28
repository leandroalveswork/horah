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
}