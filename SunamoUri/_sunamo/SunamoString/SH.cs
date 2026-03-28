namespace SunamoUri._sunamo.SunamoString;

/// <summary>
/// Provides general string utility methods.
/// </summary>
internal class SH
{
    /// <summary>
    /// Replaces only the first occurrence of the specified pattern.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="what">The pattern to find.</param>
    /// <param name="replacement">The replacement string.</param>
    /// <returns>The text with the first occurrence replaced.</returns>
    internal static string ReplaceOnce(string text, string what, string replacement)
    {
        return new Regex(what).Replace(text, replacement, 1);
    }

    /// <summary>
    /// Inserts a character before each uppercase letter in the text.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="add">The character to insert.</param>
    /// <param name="isPreservingAcronyms">Whether to preserve consecutive uppercase letters as acronyms.</param>
    /// <returns>The text with characters inserted before uppercase letters.</returns>
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

    /// <summary>
    /// Extracts leading whitespace characters from the text.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <returns>A string containing only the leading whitespace.</returns>
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

    /// <summary>
    /// Adds a prefix to the text if it does not already start with it.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="prefix">The prefix to add.</param>
    /// <param name="isSkippingWhitespaces">Whether to skip leading whitespace before checking.</param>
    /// <returns>The text with the prefix added if not already present.</returns>
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

    /// <summary>
    /// Adds a postfix to the text if the text is not empty and does not already end with it.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="postfix">The postfix to add.</param>
    /// <returns>The text with the postfix added if applicable.</returns>
    internal static string PostfixIfNotEmpty(string text, string postfix)
    {
        if (text.Length != 0)
            if (!text.EndsWith(postfix))
                return text + postfix;
        return text;
    }
}
