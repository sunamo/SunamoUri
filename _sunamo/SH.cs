namespace SunamoUri;

//namespace SunamoUri;

public class SH
{
    public static string ReplaceOnce(string input, string what, string zaco)
    {
        return new Regex(what).Replace(input, zaco, 1);
    }
    public static string AddBeforeUpperChars(string text, char add, bool preserveAcronyms)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
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

    public static string WhiteSpaceFromStart(string v)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in v)
        {
            if (char.IsWhiteSpace(item))
            {
                sb.Append(item);
            }
            else
            {
                break;
            }
        }
        return sb.ToString();
    }

    public static string PrefixIfNotStartedWith(string item, string http, bool skipWhitespaces = false)
    {
        string whitespaces = string.Empty;

        if (skipWhitespaces)
        {
            whitespaces = WhiteSpaceFromStart(item);
            item = item.Substring(whitespaces.Length);
        }

        if (!item.StartsWith(http))
        {
            return whitespaces + http + item;
        }

        return whitespaces + item;
    }
    public static string PostfixIfNotEmpty(string text, string postfix)
    {
        if (text.Length != 0)
        {
            if (!text.EndsWith(postfix))
            {
                return text + postfix;
            }
        }
        return text;
    }

    //    public static Func<string, string, string> PostfixIfNotEmpty;
    //    public static Func<string, string> FirstCharUpper;
    //    public static Func<string, string, bool, string> KeepAfterFirst;
    //    public static Func<string, string> TextWithoutDiacritic;
    //    public static Func<string, string, string, string> ReplaceOnce;
    //    public static Func<string, char, string> RemoveAfterFirstChar;
    //    public static Func<string, string, string> RemoveAfterFirst;
    //    public static Func<string, String[], List<string>> SplitMore;
    //    public static Func<string, string, List<string>> Split;
    //    public static Func<string, string, string[], string> ReplaceAll;
    //    public static Func<string, string, string> TrimEnd;
    //    public static Func<string, char, bool, string> AddBeforeUpperChars;
    //    public static Func<string, int, int, SubstringArgs, string> Substring;
    //    public static Func<string, char, List<string>> SplitChar;
    //    public static Func<string, string, bool, string> PrefixIfNotStartedWith;
    //    public static Func<string, string, string> TrimStart;
    //    public static Func<string, string> RemoveLastChar;
}
