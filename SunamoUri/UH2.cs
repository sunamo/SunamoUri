namespace SunamoUri;

public partial class UH
{
    /// <summary>
    /// Remove prefix http or https.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="protocol">The protocol.</param>
    /// <returns>The remove prefix http or https result.</returns>
    public static string RemovePrefixHttpOrHttps(string text, out string protocol)
    {
        if (text.Contains("http://"))
        {
            protocol = "http://";
            text = text.Replace("http://", "");
            return text;
        }

        if (text.Contains("https://"))
        {
            protocol = "https://";
            text = text.Replace("https://", "");
            return text;
        }

        protocol = "";
        return text;
    }

    /// <summary>
    /// Is uri.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="text">The text.</param>
    /// <returns>The is uri result.</returns>
    public static bool IsUri(ILogger logger, string text)
    {
        var uri = CreateUri(logger, text);
        return uri is not null;
    }
}
