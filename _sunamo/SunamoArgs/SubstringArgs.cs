namespace SunamoUri;


/// <summary>
/// 23-1-23 jsem ho přesunul zpět do SunamoStringSubstring, aby neměl už žádné deps
/// </summary>
public class SubstringArgs
{
    /// <summary>
    /// Was before created this class
    /// </summary>
    public bool returnInputIfInputIsShorterThanA3 = false;
    public bool returnInputIfIndexFromIsLessThanIndexTo = false;
    public static SubstringArgs Instance = new SubstringArgs();
}