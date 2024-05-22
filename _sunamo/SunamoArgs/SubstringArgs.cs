namespace SunamoUri;


/// <summary>
/// 23-1-23 jsem ho přesunul zpět do SunamoStringSubstring, aby neměl už žádné deps
/// </summary>
internal class SubstringArgs
{
    /// <summary>
    /// Was before created this class
    /// </summary>
    internal bool returnInputIfInputIsShorterThanA3 = false;
    internal bool returnInputIfIndexFromIsLessThanIndexTo = false;
    internal static SubstringArgs Instance = new SubstringArgs();
}