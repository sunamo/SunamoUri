namespace SunamoUri._sunamo.SunamoStringSubstring;

/// <summary>
/// Provides substring utility methods with additional safety checks.
/// </summary>
internal class SHSubstring
{
    /// <summary>
    /// Returns a substring between two indices with configurable error handling.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="indexFrom">The starting index.</param>
    /// <param name="indexTo">The ending index.</param>
    /// <param name="substringArgs">Optional arguments controlling behavior for edge cases.</param>
    /// <returns>The extracted substring, or empty string if indices are out of range.</returns>
    internal static string? Substring(string? text, int indexFrom, int indexTo, SubstringArgs? substringArgs = null)
    {
        if (substringArgs == null) substringArgs = SubstringArgs.Instance;

        if (text == null) return null;

        var textLength = text.Length;

        if (indexFrom > indexTo)
        {
            if (substringArgs.ShouldReturnInputWhenIndexFromExceedsIndexTo)
                return text;
            ThrowEx.ArgumentOutOfRangeException("indexFrom", "indexFrom is lower than indexTo");
        }

        if (textLength > indexFrom)
        {
            if (textLength > indexTo)
            {
                return text.Substring(indexFrom, indexTo - indexFrom);
            }

            if (substringArgs.ShouldReturnInputWhenShorterThanIndexTo) return text;
        }

        return string.Empty;
    }
}
