namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhListaSuporteStrategy
{
    public static string BoolAnulavelParaValueDeOption(bool? b)
    {
        if (b == null)
        {
            return "";
        }
        if (b == true)
        {
            return "S";
        }
        return "N";
    }

    public static bool? ValueDeOptionParaBoolAnulavel(string? vlOp)
    {
        if (string.IsNullOrEmpty(vlOp))
        {
            return null;
        }
        if (vlOp == "S")
        {
            return true;
        }
        return false;
    }
}