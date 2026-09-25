namespace SunamoUri._sunamo.SunamoStringParts;

/// <summary>
/// Provides string manipulation methods for extracting parts of strings.
/// </summary>
internal class SHParts
{
    /// <summary>
    /// Removes everything after the first occurrence of the specified delimiter character.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="delimiter">The delimiter character.</param>
    /// <returns>The text with everything after the first delimiter removed.</returns>
    internal static string RemoveAfterFirstChar(string text, char delimiter)
    {
        return RemoveAfterFirst(text, delimiter.ToString());
    }

    /// <summary>
    /// Removes the specified prefix from the start of the text repeatedly.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="prefix">The prefix to remove.</param>
    /// <returns>The text with the prefix removed.</returns>
    internal static string TrimStart(string text, string prefix)
    {
        while (text.StartsWith(prefix)) text = text.Substring(prefix.Length);

        return text;
    }

    /// <summary>
    /// Returns the part of the text after the first occurrence of the specified delimiter.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="delimiter">The delimiter to search for.</param>
    /// <param name="isKeepingDelimiter">Whether to keep the delimiter in the result.</param>
    /// <returns>The text after the first delimiter occurrence.</returns>
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

    /// <summary>
    /// Removes everything after the first occurrence of the specified delimiter string.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="delimiter">The delimiter string.</param>
    /// <returns>The text with everything after the first delimiter removed.</returns>
    internal static string RemoveAfterFirst(string text, string delimiter)
    {
        var delimiterIndex = text.IndexOf(delimiter);
        if (delimiterIndex == -1 || delimiterIndex == text.Length - 1) return text;

        var result = text.Remove(delimiterIndex);
        return result;
    }
}
