namespace SunamoUri;

/// <summary>
/// URI Helper class - additional methods for URI manipulation (partial class part 2).
/// </summary>
public partial class UH
{
    /// <summary>
    /// Returns the complete query string including the leading question mark.
    /// </summary>
    /// <param name="uri">The URI to extract the query from.</param>
    /// <returns>The query string including the leading question mark.</returns>
    public static string GetQueryAsHttpRequest(Uri uri)
    {
        return uri.Query;
    }

    /// <summary>
    /// Gets the page path from a URI without the query string.
    /// </summary>
    /// <param name="uri">The URI to extract the page name from.</param>
    /// <returns>The path and query without query string parameters.</returns>
    public static string GetPageNameFromUri(Uri uri)
    {
        var questionMarkIndex = uri.PathAndQuery.IndexOf('?');
        if (questionMarkIndex != -1)
            return uri.PathAndQuery.Substring(0, questionMarkIndex);
        return uri.PathAndQuery;
    }

    /// <summary>
    /// Gets the local file path portion of the URI.
    /// Returns the same result as GetPageNameFromUri.
    /// </summary>
    /// <param name="uri">The URI to extract the path from.</param>
    /// <returns>The local path of the URI.</returns>
    public static string GetFilePathAsHttpRequest(Uri uri)
    {
        return uri.LocalPath;
    }

    /// <summary>
    /// Gets the protocol string (scheme with "://") from a URI.
    /// </summary>
    /// <param name="uri">The URI to extract the protocol from.</param>
    /// <returns>The protocol string (e.g., "https://").</returns>
    public static string GetProtocolString(Uri uri)
    {
        return uri.Scheme + "://";
    }

    /// <summary>
    /// Returns true if the text has an http or https protocol prefix.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <returns>True if the text starts with http:// or https://.</returns>
    public static bool HasHttpProtocol(string text)
    {
        text = text.ToLower();
        if (text.StartsWith("http://"))
            return true;
        if (text.StartsWith("https://"))
            return true;
        return false;
    }

    /// <summary>
    /// Creates a Uri instance from a text string, logging an error if creation fails.
    /// </summary>
    /// <param name="logger">The logger for error reporting.</param>
    /// <param name="text">The text to parse as a URI.</param>
    /// <returns>The created Uri, or null if parsing fails.</returns>
    public static Uri? CreateUri(ILogger logger, string text)
    {
        try
        {
            return new Uri(text);
        }
        catch (Exception)
        {
            logger.LogError("Can't construct url from " + text);
            return null;
        }
    }

    /// <summary>
    /// Gets or sets the last URL-decoded result from IsUrlEncoded.
    /// </summary>
    public static string? UrlDecoded { get; set; }

    /// <summary>
    /// Checks whether a URI is URL-encoded.
    /// The decoded result is stored in UrlDecoded.
    /// </summary>
    /// <param name="uri">The URI to check.</param>
    /// <returns>True if the URI is URL-encoded.</returns>
    public static bool IsUrlEncoded(string uri)
    {
        UrlDecoded = UrlDecode(uri);
        return UrlDecoded != uri;
    }

    /// <summary>
    /// Converts a host URI to PascalCase convention.
    /// </summary>
    /// <param name="logger">The logger for error reporting.</param>
    /// <param name="text">The URI text to convert.</param>
    /// <returns>The host name in PascalCase.</returns>
    public static string HostUriToPascalConvention(ILogger logger, string text)
    {
        var uri = CreateUri(logger, text)!;
        var result = SHReplace.ReplaceAll(uri.Host, " ", ".");
        result = CaseConverter.CamelCase.ConvertCase(result);
        var stringBuilder = new StringBuilder(result);
        stringBuilder[0] = char.ToUpper(stringBuilder[0]);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Converts a title to a URI-safe string, variant 2 with different ordering of operations.
    /// </summary>
    /// <param name="title">The title to convert.</param>
    /// <returns>A URI-safe slug string.</returns>
    private static string GetUriSafeString2(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "";
        title = Regex.Replace(title, @"&\w+;", "");
        title = Regex.Replace(title, @"[^A-Za-z0-9\-\s]", "");
        title = title.Trim();
        title = Regex.Replace(title, @"\s+", "-");
        title = Regex.Replace(title, @"\-{2,}", "-");
        title = title.ToLower();
        if (title.Length > 80)
            title = title.Substring(0, 79);
        if (title.EndsWith("-"))
            title = title.Substring(0, title.Length - 1);
        return title;
    }

    /// <summary>
    /// Inserts a text segment between the path and file name in a URI.
    /// </summary>
    /// <param name="uri">The URI to modify.</param>
    /// <param name="textToInsert">The text segment to insert.</param>
    /// <returns>The modified URI with the inserted segment.</returns>
    public static string InsertBetweenPathAndFile(string uri, string textToInsert)
    {
        var segments = uri.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        segments[segments.Count - 2] += "/" + textToInsert;
        var result = Join(segments.ToArray());
        return result.Replace(":/", "://");
    }

    /// <summary>
    /// Checks if a URI matches specified hostname, path, and query string conditions.
    /// </summary>
    /// <param name="logger">The logger for error reporting.</param>
    /// <param name="source">The source URI to check.</param>
    /// <param name="hostnameEndsWith">The expected hostname suffix.</param>
    /// <param name="pathContains">The expected path substring.</param>
    /// <param name="queryStringContainsAll">All query string values that must be present.</param>
    /// <returns>True if all conditions match.</returns>
    public static bool Contains(ILogger logger, Uri source, string hostnameEndsWith, string pathContains, params string[] queryStringContainsAll)
    {
        hostnameEndsWith = hostnameEndsWith.ToLower();
        pathContains = pathContains.ToLower();
        var uri = CreateUri(logger, source.ToString().ToLower())!;
        if (uri.Host.EndsWith(hostnameEndsWith))
            if (GetFilePathAsHttpRequest(uri).Contains(pathContains))
                foreach (var item in queryStringContainsAll)
                {
                    if (!uri.Query.Contains(item))
                        return false;
                    return true;
                }

        return false;
    }

    /// <summary>
    /// Removes the tracking part (utm parameters) from a URL and returns only the domain with protocol.
    /// </summary>
    /// <param name="text">The URL to clean.</param>
    /// <returns>The URL without tracking parameters.</returns>
    public static string RemoveTrackingPart(string text)
    {
        var result = SHParts.RemoveAfterFirst(text, "#utm_");
        result = RemovePrefixHttpOrHttps(result);
        result = SHParts.RemoveAfterFirstChar(result, '/');
        if (result.Contains('.'))
            return "https://" + result;
        return result;
    }

    /// <summary>
    /// Validates whether the text is a valid URI and its domain matches the expected domain.
    /// </summary>
    /// <param name="text">The text to validate as a URI.</param>
    /// <param name="domain">The expected domain, or "*" to accept any domain.</param>
    /// <param name="isSurelyDomain">Set to true if the URI was confirmed as a valid domain.</param>
    /// <returns>True if the text is a valid URI with the expected domain.</returns>
    public static bool IsValidUriAndDomainIs(string text, string domain, out bool isSurelyDomain)
    {
        var textWithHttp = AppendHttpIfNotExists(text);
        isSurelyDomain = false;
        if (Uri.TryCreate(textWithHttp, UriKind.Absolute, out var uri))
            if (uri.Host == domain || domain == "*")
                return true;
        return false;
    }

    /// <summary>
    /// Gets the host name from a URI text.
    /// </summary>
    /// <param name="logger">The logger for error reporting.</param>
    /// <param name="text">The URI text.</param>
    /// <returns>The host name.</returns>
    public static string GetHost(ILogger logger, string text)
    {
        var uri = CreateUri(logger, AppendHttpIfNotExists(text))!;
        return uri.Host;
    }

    /// <summary>
    /// Gets the directory path from a URI (the path up to the last slash).
    /// Returns the path with a trailing slash by convention.
    /// </summary>
    /// <param name="text">The URI to extract the directory from.</param>
    /// <returns>The directory path with trailing slash.</returns>
    public static string GetDirectoryName(string text)
    {
        if (text != "/")
            text = text.TrimEnd('/');
        text = SHParts.RemoveAfterFirstChar(text, '?');
        var lastSlashIndex = text.LastIndexOf('/');
        if (lastSlashIndex != -1)
            return text.Substring(0, lastSlashIndex + 1);
        return text;
    }

    /// <summary>
    /// Gets the file name without extension from a URI.
    /// </summary>
    /// <param name="text">The URI to extract the file name from.</param>
    /// <returns>The file name without its extension.</returns>
    public static string GetFileNameWithoutExtension(string text)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(text));
    }

    /// <summary>
    /// Combines URI segments with a slash separator, handling trailing slashes.
    /// </summary>
    /// <param name="isDirectory">Whether to append a trailing slash for directory paths.</param>
    /// <param name="segments">The URI segments to combine.</param>
    /// <returns>The combined URI.</returns>
    public static string Combine(bool isDirectory, params string[] segments)
    {
        var result = string.Join('/', segments).Replace("///", "/").Replace("//", "/").TrimEnd('/').Replace(":/", "://");
        if (isDirectory)
            result += "/";
        return result;
    }

    /// <summary>
    /// Joins path segments with a slash separator.
    /// </summary>
    /// <param name="segments">The segments to join.</param>
    /// <returns>The joined path.</returns>
    private static string Join(params string[] segments)
    {
        return string.Join('/', segments);
    }

    /// <summary>
    /// Combines URI segments with automatic trailing slash handling.
    /// </summary>
    /// <param name="segments">The URI segments to combine.</param>
    /// <returns>The combined URI.</returns>
    public static string Combine(params string[] segments)
    {
        return Combine(segments.ToList());
    }

    /// <summary>
    /// Combines a list of URI segments, adding trailing slashes to non-file segments.
    /// </summary>
    /// <param name="list">The list of URI segments to combine.</param>
    /// <returns>The combined URI.</returns>
    public static string Combine(IList<string> list)
    {
        var result = new StringBuilder();
        var index = 0;
        foreach (var item in list)
        {
            index++;
            if (string.IsNullOrWhiteSpace(item))
                continue;
            if (item[item.Length - 1] == '/')
            {
                result.Append(item);
            }
            else
            {
                if (index == list.Count && Path.GetExtension(item) != "")
                    result.Append(item);
                else
                    result.Append(item + '/');
            }
        }

        return result.ToString();
    }
}
