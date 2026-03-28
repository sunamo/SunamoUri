namespace SunamoUri;

/// <summary>
/// URI Helper class - additional methods (partial class part 3).
/// </summary>
public partial class UH
{
    /// <summary>
    /// Removes http:// or https:// prefix from the text, outputting the removed protocol.
    /// If no protocol is present, protocol is set to empty string.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <param name="protocol">The removed protocol string including "://", or empty if none found.</param>
    /// <returns>The text without the protocol prefix.</returns>
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
    /// Checks whether the text can be parsed as a valid URI.
    /// </summary>
    /// <param name="logger">The logger for error reporting.</param>
    /// <param name="text">The text to validate as a URI.</param>
    /// <returns>True if the text is a valid URI.</returns>
    public static bool IsUri(ILogger logger, string text)
    {
        var uri = CreateUri(logger, text);
        return uri != null;
    }
}
