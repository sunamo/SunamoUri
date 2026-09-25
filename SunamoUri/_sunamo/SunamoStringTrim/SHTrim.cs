namespace SunamoUri._sunamo.SunamoStringTrim;

internal class SHTrim
{
    internal static string TrimEnd(string text, string suffix)
    {
        while (text.EndsWith(suffix)) return text.Substring(0, text.Length - suffix.Length);

        return text;
    }

    internal static string TrimStart(string text, string prefix)
    {
        while (text.StartsWith(prefix)) text = text.Substring(prefix.Length);

        return text;
    }
}
