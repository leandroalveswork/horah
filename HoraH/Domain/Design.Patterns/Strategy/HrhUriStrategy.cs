using Microsoft.AspNetCore.Components;

namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhUriStrategy
{
    public static string PegarParametroAsString(NavigationManager uriHelper, string key)
    {
        var href = uriHelper.BaseUri;
        if (!href.Contains($"{key}="))
        {
            return "";
        }
        var idxKey = href.IndexOf($"{key}=");
        var value = "";
        for (int iVl = idxKey + key.Length + 1; iVl < href.Length && href[iVl] != '&'; iVl++)
        {
            value += href[iVl];
        }
        return value;
    }
}