namespace SunamoUri._sunamo.SunamoString;

internal class SH
{
    internal static string ReplaceOnce(string input, string what, string zaco)
    {
        return new Regex(what).Replace(input, zaco, 1);
    }

    internal static string AddBeforeUpperChars(string text, char add, bool preserveAcronyms)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        var newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (var i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != add && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(add);
            newText.Append(text[i]);
        }

        return newText.ToString();
    }

    internal static string WhiteSpaceFromStart(string v)
    {
        var sb = new StringBuilder();
        foreach (var item in v)
            if (char.IsWhiteSpace(item))
                sb.Append(item);
            else
                break;
        return sb.ToString();
    }

    internal static string PrefixIfNotStartedWith(string item, string http, bool skipWhitespaces = false)
    {
        var whitespaces = string.Empty;

        if (skipWhitespaces)
        {
            whitespaces = WhiteSpaceFromStart(item);
            item = item.Substring(whitespaces.Length);
        }

        if (!item.StartsWith(http)) return whitespaces + http + item;

        return whitespaces + item;
    }

    internal static string PostfixIfNotEmpty(string text, string postfix)
    {
        if (text.Length != 0)
            if (!text.EndsWith(postfix))
                return text + postfix;
        return text;
    }

    //    internal static Func<string, string, string> PostfixIfNotEmpty;
    //    internal static Func<string, string> FirstCharUpper;
    //    internal static Func<string, string, bool, string> KeepAfterFirst;
    //    internal static Func<string, string> TextWithoutDiacritic;
    //    internal static Func<string, string, string, string> ReplaceOnce;
    //    internal static Func<string, char, string> RemoveAfterFirstChar;
    //    internal static Func<string, string, string> RemoveAfterFirst;
    //    internal static Func<string, String[], List<string>> SplitMore;
    //    internal static Func<string, string, List<string>> Split;
    //    internal static Func<string, string, string[], string> ReplaceAll;
    //    internal static Func<string, string, string> TrimEnd;
    //    internal static Func<string, char, bool, string> AddBeforeUpperChars;
    //    internal static Func<string, int, int, SubstringArgs, string> Substring;
    //    internal static Func<string, char, List<string>> SplitChar;
    //    internal static Func<string, string, bool, string> PrefixIfNotStartedWith;
    //    internal static Func<string, string, string> TrimStart;
    //    internal static Func<string, string> RemoveLastChar;
    internal static string JoinNL(List<string> l)
    {
        StringBuilder sb = new();
        foreach (var item in l) sb.AppendLine(item);
        var r = string.Empty;
        r = sb.ToString();
        return r;
    }

    internal static List<string> SplitCharMore(string s, params char[] dot)
    {
        return s.Split(dot, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    internal static List<string> SplitMore(string s, params string[] dot)
    {
        return s.Split(dot, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    internal static List<string> SplitNone(string text, params string[] deli)
    {
        return text.Split(deli, StringSplitOptions.None).ToList();
    }

    internal static string NullToStringOrDefault(object n)
    {
        return n == null ? " " + "(null)" : " " + n;
    }

    internal static string TrimEnd(string name, string ext)
    {
        while (name.EndsWith(ext)) return name.Substring(0, name.Length - ext.Length);
        return name;
    }
}