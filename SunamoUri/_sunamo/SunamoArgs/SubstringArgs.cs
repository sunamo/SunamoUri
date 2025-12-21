namespace SunamoUri._sunamo.SunamoArgs;

/// <summary>
///     23-1-23 jsem ho přesunul zpět do SunamoStringSubstring, aby neměl už žádné deps
/// </summary>
internal class SubstringArgs
{
    internal static SubstringArgs Instance = new();
    internal bool returnInputIfIndexFromIsLessThanIndexTo = false;

    /// <summary>
    ///     Was before created this class
    /// </summary>
    internal bool returnInputIfInputIsShorterThanA3 = false;
}