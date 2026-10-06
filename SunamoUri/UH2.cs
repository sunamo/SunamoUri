namespace SunamoUri;

public partial class UH
{
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

    public static bool IsUri(ILogger logger, string text)
    {
        var uri = CreateUri(logger, text);
        return uri is not null;
    }
}
