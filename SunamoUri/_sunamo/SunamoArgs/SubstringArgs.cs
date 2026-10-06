namespace SunamoUri._sunamo.SunamoArgs;

internal class SubstringArgs
{
    internal static SubstringArgs Instance { get; set; } = new();

    internal bool ShouldReturnInputWhenIndexFromExceedsIndexTo { get; set; } = false;

    internal bool ShouldReturnInputWhenShorterThanIndexTo { get; set; } = false;
}
