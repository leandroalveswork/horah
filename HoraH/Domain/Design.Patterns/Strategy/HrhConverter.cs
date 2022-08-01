using HoraH.Domain.Design.Patterns.Strategy.Md;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoraH.Domain.Design.Patterns.Strategy;
public class HrhConverter
{
    public static List<SelectListItem> ParaListaDeItensDeDropdownDeDicionario<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase) where TValor : IComparable
    {
        return dicionarioBase
            .Select(itemNoDicionario => new SelectListItem(itemNoDicionario.Label, itemNoDicionario.Indice.ToString()))
            .ToList();
    }

    public static SelectListItem ParaItemDeDropdownDeDicionario<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase, TValor valorDicionario) where TValor : IComparable
    {
        foreach (var iParChaveValor in dicionarioBase)
        {
            if (iParChaveValor.ValorLogico.CompareTo(valorDicionario) == 0)
            {
                return new SelectListItem(iParChaveValor.Label, iParChaveValor.Indice.ToString());
            }
        }
        throw new ArgumentException("O valor em 'valorDicionario' não foi encontrado no dicionário de valores 'dicionarioBase'");
    }

    public static TValor ParaValorDeDicionarioUsandoValue<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase, string valueDoItemDeDropdownDeDicionario) where TValor : IComparable
    {
        foreach (var iParChaveValor in dicionarioBase)
        {
            if (iParChaveValor.Indice.ToString() == valueDoItemDeDropdownDeDicionario)
            {
                return iParChaveValor.ValorLogico;
            }
        }
        throw new IndexOutOfRangeException("O dicionário de valores 'dicionarioBase' não possui o índice indicado por 'valueDoItemDeDropdownDeDicionario'");
    }

    public static TValor ParaValorDeDicionario<TValor>(IEnumerable<HrhMdItemDic<TValor>> dicionarioBase, SelectListItem itemDeDropdownDeDicionario) where TValor : IComparable
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

    public static List<HrhMdItemDic<bool?>> CriarDicionarioDeValoresNullableBool()
    {
        return new List<HrhMdItemDic<bool?>>()
        {
            new HrhMdItemDic<bool?> { Indice = 0, ValorLogico = null, Label = "Selecione..." },
            new HrhMdItemDic<bool?> { Indice = 1, ValorLogico = true, Label = "Sim" },
            new HrhMdItemDic<bool?> { Indice = 2, ValorLogico = false, Label = "Não" }
        };
    }
}