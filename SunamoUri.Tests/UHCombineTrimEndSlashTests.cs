namespace SunamoUri.Tests;

/// <summary>
/// Tests for UH.CombineTrimEndSlash.
/// </summary>
public class UHCombineTrimEndSlashTests
{
    /// <summary>
    /// Tests that a single URI is returned without a trailing slash.
    /// </summary>
    [Fact]
    public void CombineTrimEndSlash_SingleSegment()
    {
        Assert.Equal("https://www.sauto.cz/osobni/detail", UH.CombineTrimEndSlash("https://www.sauto.cz/osobni/detail/"));
        Assert.Equal("https://www.sauto.cz/osobni/detail", UH.CombineTrimEndSlash("https://www.sauto.cz/osobni/detail"));
    }

    /// <summary>
    /// Tests that segments are joined with one slash, empty segments are skipped and no trailing slash is added.
    /// </summary>
    [Fact]
    public void CombineTrimEndSlash_ParamsSegments()
    {
        Assert.Equal("https://www.sauto.cz/osobni/detail/skoda", UH.CombineTrimEndSlash("https://www.sauto.cz/", "/osobni", "", "detail/", "skoda"));
    }

    /// <summary>
    /// Tests that the list overload gives the same result as the params overload, also for a last segment with a query string.
    /// </summary>
    [Fact]
    public void CombineTrimEndSlash_ListSegments()
    {
        var segments = new List<string> { "https://www.sauto.cz", "osobni", "detail", "123?goFrom=list" };

        Assert.Equal("https://www.sauto.cz/osobni/detail/123?goFrom=list", UH.CombineTrimEndSlash(segments));
    }
}
