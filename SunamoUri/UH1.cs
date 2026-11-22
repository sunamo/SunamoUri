namespace SunamoUri;

public partial class UH
{
    /// <summary>
    /// Get query as http request.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <returns>The get query as http request result.</returns>
    public static string GetQueryAsHttpRequest(Uri uri)
    {
        return uri.Query;
    }

    /// <summary>
    /// Get page name from uri.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <returns>The get page name from uri result.</returns>
    public static string GetPageNameFromUri(Uri uri)
    {
        var questionMarkIndex = uri.PathAndQuery.IndexOf('?');
        if (questionMarkIndex != -1)
            return uri.PathAndQuery.Substring(0, questionMarkIndex);
        return uri.PathAndQuery;
    }

    /// <summary>
    /// Get file path as http request.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <returns>The get file path as http request result.</returns>
    public static string GetFilePathAsHttpRequest(Uri uri)
    {
        return uri.LocalPath;
    }

    /// <summary>
    /// Get protocol string.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <returns>The get protocol string result.</returns>
    public static string GetProtocolString(Uri uri)
    {
        return uri.Scheme + "://";
    }

    /// <summary>
    /// Has http protocol.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The has http protocol result.</returns>
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
    /// Create uri.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="text">The text.</param>
    /// <returns>The create uri result.</returns>
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
    /// Gets or sets the url decoded.
    /// </summary>
    public static string? UrlDecoded { get; set; }

    /// <summary>
    /// Is url encoded.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <returns>The is url encoded result.</returns>
    public static bool IsUrlEncoded(string uri)
    {
        UrlDecoded = UrlDecode(uri);
        return UrlDecoded != uri;
    }

    /// <summary>
    /// Host uri to pascal convention.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="text">The text.</param>
    /// <returns>The host uri to pascal convention result.</returns>
    public static string HostUriToPascalConvention(ILogger logger, string text)
    {
        var uri = CreateUri(logger, text)!;
        var result = SHReplace.ReplaceAll(uri.Host, " ", ".");
        result = CaseConverter.CamelCase.ConvertCase(result);
        var stringBuilder = new StringBuilder(result);
        stringBuilder[0] = char.ToUpper(stringBuilder[0]);
        return stringBuilder.ToString();
    }

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
    /// Insert between path and file.
    /// </summary>
    /// <param name="uri">The uri.</param>
    /// <param name="textToInsert">The text to insert.</param>
    /// <returns>The insert between path and file result.</returns>
    public static string InsertBetweenPathAndFile(string uri, string textToInsert)
    {
        var segments = uri.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        segments[segments.Count - 2] += "/" + textToInsert;
        var result = Join(segments.ToArray());
        return result.Replace(":/", "://");
    }

    /// <summary>
    /// Contains.
    /// </summary>
    /// <param name="":/"">The ":/".</param>
    /// <param name=""://"">The "://".</param>
    /// <returns>The contains result.</returns>
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
    /// Remove tracking part.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The remove tracking part result.</returns>
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
    /// Is valid uri and domain is.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="domain">The domain.</param>
    /// <param name="isSurelyDomain">The is surely domain.</param>
    /// <returns>The is valid uri and domain is result.</returns>
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
    /// Get host.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="text">The text.</param>
    /// <returns>The get host result.</returns>
    public static string GetHost(ILogger logger, string text)
    {
        var uri = CreateUri(logger, AppendHttpIfNotExists(text))!;
        return uri.Host;
    }

    /// <summary>
    /// Get directory name.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The get directory name result.</returns>
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
    /// Get file name without extension.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The get file name without extension result.</returns>
    public static string GetFileNameWithoutExtension(string text)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(text));
    }

    /// <summary>
    /// Combine.
    /// </summary>
    /// <param name="GetFileName(text)">The get file name text.</param>
    /// <returns>The combine result.</returns>
    public static string Combine(bool isDirectory, params string[] segments)
    {
        var result = string.Join("/", segments).Replace("///", "/").Replace("//", "/").TrimEnd('/').Replace(":/", "://");
        if (isDirectory)
            result += "/";
        return result;
    }

    private static string Join(params string[] segments)
    {
        return string.Join("/", segments);
    }

    /// <summary>
    /// Combine.
    /// </summary>
    /// <param name=""/"">The "/".</param>
    /// <param name="segments">The segments.</param>
    /// <returns>The combine result.</returns>
    public static string Combine(params string[] segments)
    {
        return Combine(segments.ToList());
    }

    /// <summary>
    /// Combine.
    /// </summary>
    /// <param name="segments.ToList()">The segments to list.</param>
    /// <returns>The combine result.</returns>
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
