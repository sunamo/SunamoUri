namespace SunamoUri._sunamo.SunamoString;

internal class SH
{
    internal static string ReplaceOnce(string text, string what, string replacement)
    {
        return new Regex(what).Replace(text, replacement, 1);
    }

    internal static string AddBeforeUpperChars(string text, char add, bool isPreservingAcronyms)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        var newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (var i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != add && !char.IsUpper(text[i - 1])) ||
                    (isPreservingAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(add);
            newText.Append(text[i]);
        }

        return newText.ToString();
    }

    internal static string WhiteSpaceFromStart(string text)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in text)
            if (char.IsWhiteSpace(item))
                stringBuilder.Append(item);
            else
                break;
        return stringBuilder.ToString();
    }

    internal static string PrefixIfNotStartedWith(string text, string prefix, bool isSkippingWhitespaces = false)
    {
        var whitespaces = string.Empty;

        if (isSkippingWhitespaces)
        {
            whitespaces = WhiteSpaceFromStart(text);
            text = text.Substring(whitespaces.Length);
        }

        if (!text.StartsWith(prefix)) return whitespaces + prefix + text;

        return whitespaces + text;
    }

    internal static string PostfixIfNotEmpty(string text, string postfix)
    {
        if (text.Length != 0)
            if (!text.EndsWith(postfix))
                return text + postfix;
        return text;
    }
}
