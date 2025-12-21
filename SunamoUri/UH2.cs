namespace SunamoUri;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class UH
{
    /// <summary>
    ///     value parameter��pad� �e value A1 nebude protokol, ulo�� se do A2 ""
    ///     value parameter��pad� �e tam protokol bude, ulo�� se do A2 value�etn� ://
    /// </summary>
    /// <param name = "t"></param>
    /// <param name = "protocol"></param>
    public static string RemovePrefixHttpOrHttps(string t, out string protocol)
    {
        if (t.Contains("http://"))
        {
            protocol = "http://";
            t = t.Replace("http://", "");
            return t;
        }

        if (t.Contains("https://"))
        {
            protocol = "https://";
            t = t.Replace("https://", "");
            return t;
        }

        protocol = "";
        return t;
    }

    /// <summary>
    ///     pass also for page:
    /// </summary>
    /// <param name = "href"></param>
    /// <returns></returns>
    public static bool IsUri(ILogger logger, string href)
    {
        var uri = CreateUri(logger, href);
        return uri != null;
    }
}