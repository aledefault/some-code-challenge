using System.Text.RegularExpressions;

namespace ReelWords;

// Not the best implementation, but it's the cleanest (not injected) way of deal with GeneratedRegex.
public static partial class WordsValidator
{

    [GeneratedRegex("^[a-zA-Zà-üÀ-Ü]+?[a-zA-Zà-üÀ-Ü']*?$", RegexOptions.Compiled)]
    public static partial Regex ValidationRegex();
}