namespace HoraH.Domain.Design.Patterns.Strategy;
public static class Uteis
{
    public static string RemoverAcentuacao(this string texto)
    {
        var comAcento = "áàãâäéèêëíìîïóòõôöúùûüñÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÔÖÚÙÛÜÑ";
        var semAcento = "aaaaaeeeeiiiiooooouuuunAAAAAEEEEIIIIOOOOOUUUUN";
        var textoSemAcentos = texto;
        for (var i = 0; i < comAcento.Length; i++)
        {
            textoSemAcentos = textoSemAcentos.Replace(comAcento[i], semAcento[i]);
        }
        return textoSemAcentos;
    }
}