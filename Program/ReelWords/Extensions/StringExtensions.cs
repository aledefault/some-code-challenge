using System.Globalization;
using System.Linq;
using System.Text;

namespace ReelWords.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Normalize a string, removing its accent marks and lowering its case.
    /// This is an easy way to sanitize our data. A better approach would be to inject it by DI.
    /// </summary>
    /// <param name="subject"></param>
    /// <returns>Our normalize string.</returns>
    public static string NormalizeFormat(this string subject)
    {
        //return subject.ToLowerInvariant(); // In case you want to play with accents

        char[] result = new char[subject.Length];
        return new string(subject.ToLowerInvariant().Normalize(NormalizationForm.FormD)
          .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
          .ToArray());
    }
}
