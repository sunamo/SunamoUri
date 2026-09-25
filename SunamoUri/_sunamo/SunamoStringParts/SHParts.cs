namespace SunamoUri._sunamo.SunamoStringParts;

internal class SHParts
{
    internal static string RemoveAfterFirstChar(string text, char delimiter)
    {
        return RemoveAfterFirst(text, delimiter.ToString());
    }

    internal static string TrimStart(string text, string prefix)
    {
        while (text.StartsWith(prefix)) text = text.Substring(prefix.Length);

        return text;
    }

    internal static string KeepAfterFirst(string text, string delimiter, bool isKeepingDelimiter = false)
    {
        var delimiterIndex = text.IndexOf(delimiter);
        if (delimiterIndex != -1)
        {
            text = TrimStart(text.Substring(delimiterIndex), delimiter);
            if (isKeepingDelimiter) text = delimiter + text;
        }

        return text;
    }

    internal static string RemoveAfterFirst(string text, string delimiter)
    {
        var delimiterIndex = text.IndexOf(delimiter);
        if (delimiterIndex == -1 || delimiterIndex == text.Length - 1) return text;

        var result = text.Remove(delimiterIndex);
        return result;
    }
}
