using System.Text.Json;

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

    public static string AsTotalHorasFormatted(this int minutos)
    {
        var horasStr = (minutos / 60.0).ArredondarParaBaixo();
        var minutosStr = minutos % 60;
        if (horasStr == 0)
        {
            return minutosStr + "min";
        }
        return horasStr + "h" + minutosStr + "min";
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

    public static int AsStringId(this string s)
    {
        var soma = 0;
        var fator = 7;
        var potencia10 = 1;
        // acumular soma de cada caractere da string transformado em int
        for (int idxS = 0; idxS < s.Length; idxS++)
        {
            soma += (s[idxS]) * fator * potencia10;
            // incrementar fator
            fator++;
            if (fator >= 14)
            {
                fator = 7 + (soma % 5);
            }
            // incrementar potencia 10
            potencia10 *= 10;
            if (potencia10 >= 100000)
            {
                potencia10 = 1;
            }
        }
        return soma;
    }
    
    public static string RemoverAcentuacao(this string texto)
    {
        if (texto == null)
        {
            return "";
        }
        var comAcento = "????????????????????????????????????????????????????????????????????????????????????????????";
        var semAcento = "aaaaaeeeeiiiiooooouuuunAAAAAEEEEIIIIOOOOOUUUUN";
        var textoSemAcentos = texto;
        for (var i = 0; i < comAcento.Length; i++)
        {
            textoSemAcentos = textoSemAcentos.Replace(comAcento[i], semAcento[i]);
        }
        return textoSemAcentos;
    }

    public static int ArredondarParaBaixo(this double numero)
    {
        return Convert.ToInt32(Math.Floor(numero));
    }
    
    public static int ArredondarParaCima(this double numero)
    {
        return Convert.ToInt32(Math.Ceiling(numero));
    }

    public static T AsDeserializadoJson<T>(this string stringJson)
    {
        return (T)JsonSerializer.Deserialize(stringJson, typeof(T));
    }

    public static string AsSerializadoJson<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T DuplicarNaMemoria<T>(this T objetoPorReferencia)
    {
        return objetoPorReferencia.AsSerializadoJson().AsDeserializadoJson<T>();
    }
}