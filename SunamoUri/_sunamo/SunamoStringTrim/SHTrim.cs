namespace SunamoUri._sunamo.SunamoStringTrim;

/// <summary>
/// Provides string trimming utility methods.
/// </summary>
internal class SHTrim
{
    /// <summary>
    /// Removes the specified suffix from the end of the text.
    /// </summary>
    /// <param name="text">The text to trim.</param>
    /// <param name="suffix">The suffix to remove.</param>
    /// <returns>The trimmed text.</returns>
    internal static string TrimEnd(string text, string suffix)
    {
        while (text.EndsWith(suffix)) return text.Substring(0, text.Length - suffix.Length);

        return text;
    }

    /// <summary>
    /// Removes the specified prefix from the start of the text.
    /// </summary>
    /// <param name="text">The text to trim.</param>
    /// <param name="prefix">The prefix to remove.</param>
    /// <returns>The trimmed text.</returns>
    internal static string TrimStart(string text, string prefix)
    {
        while (text.StartsWith(prefix)) text = text.Substring(prefix.Length);

        return text;
    }
}
