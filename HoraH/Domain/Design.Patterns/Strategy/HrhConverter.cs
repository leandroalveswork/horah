using HoraH.Domain.Design.Patterns.Strategy.Md;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhConverter
{
    public static List<SelectListItem> ParaListaDeItensDeDropdownDeDicionario<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase)
    {
        return dicionarioBase
            .Select(itemNoDicionario => new SelectListItem(itemNoDicionario.Label, itemNoDicionario.Indice.ToString()))
            .ToList();
    }

    public static SelectListItem ParaItemDeDropdownDeDicionario<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase, TValor valorDicionario)
    {
        foreach (var iParChaveValor in dicionarioBase)
        {
            if (iParChaveValor.CompararLogicoCom(valorDicionario) == 0)
            {
                return new SelectListItem(iParChaveValor.Label, iParChaveValor.Indice.ToString());
            }
        }
        throw new ArgumentException("valorDicionario");
    }

    public static TValor ParaValorDeDicionarioUsandoValue<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase, string valueDoItemDeDropdownDeDicionario)
    {
        foreach (var iParChaveValor in dicionarioBase)
        {
            if (iParChaveValor.Indice.ToString() == valueDoItemDeDropdownDeDicionario)
            {
                return iParChaveValor.ValorLogico;
            }
        }
        throw new IndexOutOfRangeException("valueDoItemDeDropdownDeDicionario");
    }

    public static TValor ParaValorDeDicionario<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase, SelectListItem itemDeDropdownDeDicionario)
    {
        try
        {
            return ParaValorDeDicionarioUsandoValue(dicionarioBase, itemDeDropdownDeDicionario.Value);
        }
        catch (IndexOutOfRangeException inEx)
        {
            throw new IndexOutOfRangeException("O dicionário de valores 'dicionarioBase' não possui o índice indicado por 'itemDeDropdownDeDicionario.Value'", inEx);
        }
    }

    public static List<HrhMdItemDicBool> CriarDicionarioDeSelectNullableBool()
    {
        return new List<HrhMdItemDicBool>()
        {
            new HrhMdItemDicBool { Indice = 0, ValorLogico = null, Label = "Selecione..." },
            new HrhMdItemDicBool { Indice = 1, ValorLogico = true, Label = "Sim" },
            new HrhMdItemDicBool { Indice = 2, ValorLogico = false, Label = "Não" }
        };
    }

    public static List<HrhMdItemDicSuporte> CriarDicionarioDeSelectListaSuporte(IEnumerable<HrhMdValorItemDicSuporte> itensSuporte)
    {
        return itensSuporte
            .Select((x, idxAtual) => new HrhMdItemDicSuporte { Indice = idxAtual, ValorLogico = x, Label = x.Texto })
            .ToList();
    }
}