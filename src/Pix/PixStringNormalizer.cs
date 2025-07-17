using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Pix
{
    internal static class PixStringNormalizer
    {
        public static string Normalize(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            var semAcento = sb.ToString().Normalize(NormalizationForm.FormC);

            var regex = new Regex("[^A-Z0-9 ]+");
            var upper = semAcento.ToUpperInvariant();
            var apenasValidos = regex.Replace(upper, string.Empty);

            var semEspacosExtras = Regex.Replace(apenasValidos, @"\s{2,}", " ").Trim();

            return semEspacosExtras;
        }
    }
}