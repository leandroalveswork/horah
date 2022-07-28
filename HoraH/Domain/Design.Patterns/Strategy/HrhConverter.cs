using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhConverter
{
    public static SelectListItem ParaValorDeDropdownDeBool(bool? valorBool)
    {
        if (valorBool == null)
        {
            return new SelectListItem("Selecione...", "0");
        }
        if (valorBool.Value)
        {
            return new SelectListItem("Sim", "1");
        }
        return new SelectListItem("Não", "2");
    }

    public static bool? ParaValorBool(SelectListItem valorDeDropdownDeBool)
    {
        if (valorDeDropdownDeBool.Value == "0")
        {
            return null;
        }
        if (valorDeDropdownDeBool.Value == "1")
        {
            return true;
        }
        return false;
    }

    public static List<SelectListItem> NovoDropdownDeBool()
    {
        return new List<SelectListItem>()
        {
            new SelectListItem("Selecione...", "0"),
            new SelectListItem("Sim", "1"),
            new SelectListItem("Não", "2")
        };
    }
}