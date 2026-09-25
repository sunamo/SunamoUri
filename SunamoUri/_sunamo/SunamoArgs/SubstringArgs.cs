namespace SunamoUri._sunamo.SunamoArgs;

/// <summary>
/// Configuration arguments for substring operations.
/// </summary>
internal class SubstringArgs
{
    /// <summary>
    /// Gets or sets the singleton instance with default settings.
    /// </summary>
    internal static SubstringArgs Instance { get; set; } = new();

    /// <summary>
    /// Gets or sets whether to return the original input when indexFrom exceeds indexTo.
    /// </summary>
    internal bool ShouldReturnInputWhenIndexFromExceedsIndexTo { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to return the original input when it is shorter than the specified indexTo.
    /// </summary>
    internal bool ShouldReturnInputWhenShorterThanIndexTo { get; set; } = false;
}
