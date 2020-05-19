using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Hop.Framework.Core.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static int ToInt(this string texto)
        {
            int retorno = 0;
            int.TryParse(texto, out retorno);
            return retorno;
        }

        public static decimal ToDecimal(this string texto)
        {
            decimal retorno = 0;
            decimal.TryParse(texto, out retorno);
            return retorno;
        }

        public static short ToShort(this string texto)
        {
            short retorno = 0;
            short.TryParse(texto, out retorno);
            return retorno;
        }

        public static DateTime ToDateTime(this string texto, string formato)
        {
            var data = DateTime.ParseExact(texto, formato, System.Globalization.CultureInfo.InvariantCulture);
            return data;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}
