namespace SunamoUri._sunamo.SunamoStringSubstring;

internal class SHSubstring
{
    internal static string? Substring(string? text, int indexFrom, int indexTo, SubstringArgs? substringArgs = null)
    {
        substringArgs ??= SubstringArgs.Instance;

        if (text is null) return null;

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
