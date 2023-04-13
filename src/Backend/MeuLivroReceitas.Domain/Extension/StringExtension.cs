using System.Globalization;
using System.Text;

namespace MeuLivroReceitas.Domain.Extension;

public static class StringExtension
{
    //ira extender funcao na string
    public static bool WordCompareInsensitive(this string origin, string searchFor)
    {
        //parametro 1 = onde sera pesquisado a palavra ou letra
        //parametro 2 = oque se quer pesquisar
        //parametro 3 = opcoes de comparacao
        //opcoes ignorar maiusculo e minusculo CompareOptions.IgnoreCase
        //opcoes ignorar acentos CompareOptions.IgnoreNonSpace
        //retorna valor 0 se a palavra conter oque se quer pesquisar 
        var index = CultureInfo.CurrentCulture.CompareInfo.IndexOf(origin, searchFor, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

        if (index >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static string RemoveAccents(this string text)
    {
        //quando encontra caracter especial substitui por um normal
        return new string(text.Normalize(NormalizationForm.FormD)
            .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            .ToArray());
    }
}
