namespace SunamoUri;

/// <summary>
/// URI Helper class providing methods for URL manipulation, encoding, and parsing.
/// </summary>
public partial class UH
{
    /// <summary>
    /// Removes the last character from the text.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <returns>The text without its last character.</returns>
    public static string RemoveLastChar(string text)
    {
        return text.Substring(0, text.Length - 1);
    }

    /// <summary>
    /// Extracts leading whitespace characters from the text.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <returns>A string containing only the leading whitespace.</returns>
    public static string WhiteSpaceFromStart(string text)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in text)
            if (char.IsWhiteSpace(item))
                stringBuilder.Append(item);
            else
                break;
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Removes the host and protocol from a URI, returning only the path.
    /// </summary>
    /// <param name="uri">The URI to process.</param>
    /// <returns>The path portion of the URI.</returns>
    public static string RemoveHostAndProtocol(Uri uri)
    {
        var textWithoutProtocol = RemovePrefixHttpOrHttps(uri.ToString());
        var slashIndex = textWithoutProtocol.IndexOf('/');
        return textWithoutProtocol.Substring(slashIndex);
    }

    /// <summary>
    /// Removes http:// or https:// prefix from the text.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <returns>The text without the HTTP/HTTPS prefix.</returns>
    public static string RemovePrefixHttpOrHttps(string text)
    {
        text = text.Replace("http://", "");
        text = text.Replace("https://", "");
        return text;
    }

    /// <summary>
    /// Converts the text to lowercase with the first character uppercased,
    /// then resolves it against UriShortConsts to produce a debug localhost URL.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <returns>The resolved debug URL.</returns>
    public static string DebugLocalhost(string text)
    {
        text = text.ToLower();
        var stringBuilder = new StringBuilder(text);
        stringBuilder[0] = char.ToUpper(stringBuilder[0]);
        text = stringBuilder.ToString();
        if (text != Translate.FromKey(XlfKeys.Nope))
        {
            var fields = typeof(UriShortConsts).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(fieldInfo => fieldInfo.IsLiteral && !fieldInfo.IsInitOnly).ToList();
            var matchingField = fields.Where(fieldInfo => fieldInfo.Name.StartsWith(text)).First();
            var result = "https://" + matchingField.GetValue(null) + " / ";
            return result;
        }

#if !DEBUG
        return "https://sunamo.cz";
#endif
        return "https://sunamo.net";
    }

    /// <summary>
    /// Checks if the URI is well-formed and trims it.
    /// </summary>
    /// <param name="uri">The URI to validate (modified in place).</param>
    /// <param name="uriKind">The kind of URI to validate against.</param>
    /// <returns>True if the URI is well-formed.</returns>
    public static bool IsWellFormedUriString(ref string uri, UriKind uriKind)
    {
        uri = uri.Trim();
        uri = uri.TrimEnd(':');
        var isWellFormed = Uri.IsWellFormedUriString(uri, uriKind);
        if (isWellFormed)
            uri = AppendHttpIfNotExists(uri);
        return isWellFormed;
    }

    /// <summary>
    /// Gets the pathname from a URI by removing the protocol and host.
    /// </summary>
    /// <param name="uri">The URI to extract the path from.</param>
    /// <returns>The pathname portion of the URI.</returns>
    public static string GetPathname(string uri)
    {
        uri = RemovePrefixHttpOrHttps(uri);
        uri = SHParts.KeepAfterFirst(uri, "/");
        return uri;
    }

    /// <summary>
    /// Sanitizes a URI to keep only the host name without protocol, www prefix, or path.
    /// </summary>
    /// <param name="text">The URI to sanitize.</param>
    /// <returns>The host name only.</returns>
    public static string SanitizeKeepOnlyHost(string text)
    {
        text = RemoveProtocol(text);
        text = SHParts.RemoveAfterFirstChar(text, '/');
        text = text.Replace("www.", "");
        text = text.TrimEnd('/');
        return text;
    }

    /// <summary>
    /// Removes the protocol (http: or https:) from the text.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <returns>The text without the protocol.</returns>
    private static string RemoveProtocol(string text)
    {
        text = SH.ReplaceOnce(text, "http:", "");
        text = SH.ReplaceOnce(text, "https:", "");
        return text;
    }

    /// <summary>
    /// Keeps only the host and protocol from a URI, removing the path, trailing slash, and www prefix.
    /// </summary>
    /// <param name="text">The URI to process.</param>
    /// <returns>The host with protocol only.</returns>
    public static string KeepOnlyHostAndProtocol(string text)
    {
        var parts = text.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var index = 0;
        if (parts.Count == 2)
            index = 1;
        parts[index] = SHParts.RemoveAfterFirstChar(parts[index], '/');
        return SHTrim.TrimStart(string.Join("//", parts).TrimEnd('/'), "www.");
    }

    /// <summary>
    /// Gets a path segment from a URI at the specified offset from the end.
    /// </summary>
    /// <param name="text">The URI to process.</param>
    /// <param name="offset">The negative offset from the end of the segments.</param>
    /// <returns>The path segment at the specified offset.</returns>
    public static string GetToken(string text, int offset)
    {
        var parts = text.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        return parts[parts.Count + offset];
    }

    /// <summary>
    /// Prepends https:// to the text if it does not already start with https.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <returns>The text with https:// prefix.</returns>
    public static string AppendHttpsIfNotExists(string text)
    {
        var result = text;
        if (!text.StartsWith("https"))
            result = "https://" + text;
        return result;
    }

    /// <summary>
    /// Prepends http:// to the text if it does not already start with http.
    /// </summary>
    /// <param name="text">The text to process.</param>
    /// <returns>The text with http:// prefix.</returns>
    public static string AppendHttpIfNotExists(string text)
    {
        var result = text;
        if (!text.StartsWith("http"))
            result = "http://" + text;
        return result;
    }

    /// <summary>
    /// Converts a title string to a URI-safe slug with a maximum length of 80 characters.
    /// </summary>
    /// <param name="title">The title to convert.</param>
    /// <returns>A URI-safe slug string.</returns>
    public static string GetUriSafeString(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "";
        title = SH.AddBeforeUpperChars(title, '-', false);
        title = title.RemoveDiacritics();
        title = Regex.Replace(title, @"\s+", "-");
        title = Regex.Replace(title, @"\-{2,}", "-");
        title = title.ToLower();
        title = Regex.Replace(title, @"&\w+;", "");
        title = Regex.Replace(title, @"[^a-z0-9\-\s]", "");
        title = title.Replace(' ', '-');
        title = Regex.Replace(title, @"-{2,}", "-");
        title = title.TrimStart(new[] { '-' });
        if (title.Length > 80)
            title = title.Substring(0, 79);
        title = title.TrimEnd(new[] { '-' });
        return title;
    }

    /// <summary>
    /// Ensures the host URL starts with https:// and ends with /.
    /// </summary>
    /// <param name="hostApp">The host application URL.</param>
    public static void BeforeCombine(ref string hostApp)
    {
        hostApp = SH.PrefixIfNotStartedWith(hostApp, "https://");
        hostApp = SH.PostfixIfNotEmpty(hostApp, "/");
    }

    /// <summary>
    /// Converts a title string to a URI-safe slug with a specified maximum length.
    /// </summary>
    /// <param name="title">The title to convert.</param>
    /// <param name="maxLength">The maximum length of the resulting slug.</param>
    /// <returns>A URI-safe slug string.</returns>
    public static string GetUriSafeString(string title, int maxLength)
    {
        if (string.IsNullOrEmpty(title))
            return "";
        title = title.RemoveDiacritics();
        title = Regex.Replace(title, @"\s+", "-");
        title = Regex.Replace(title, @"\-{2,}", "-");
        title = title.ToLower();
        title = Regex.Replace(title, @"&\w+;", "");
        title = Regex.Replace(title, @"[^a-z0-9\-\s]", "");
        title = title.Replace(' ', '-');
        title = Regex.Replace(title, @"-{2,}", "-");
        title = title.TrimStart(new[] { '-' });
        title = title.TrimEnd(new[] { '-' });
        title = SHReplace.ReplaceAll(title, "-", "--");
        if (title.Length > maxLength)
            title = title.Substring(0, maxLength);
        return title;
    }

    /// <summary>
    /// Converts a tag name to a URI-safe slug, ensuring uniqueness by appending a number if needed.
    /// </summary>
    /// <param name="tagName">The tag name to convert.</param>
    /// <param name="maxLength">The maximum length of the resulting slug.</param>
    /// <param name="methodInWebExists">A function that checks whether the slug already exists.</param>
    /// <returns>A unique URI-safe slug string.</returns>
    public static string GetUriSafeString(string tagName, int maxLength, Func<string, bool> methodInWebExists)
    {
        var uri = GetUriSafeString(tagName, maxLength);
        var increment = 1;
        while (methodInWebExists.Invoke(uri))
            if (uri.Length + increment.ToString().Length >= maxLength)
            {
                tagName = tagName.Substring(0, tagName.Length - 1);
            }
            else
            {
                var incrementText = increment.ToString();
                if (increment == 1)
                    incrementText = "";
                uri = GetUriSafeString(tagName + incrementText, maxLength);
                increment++;
            }

        return uri;
    }

    /// <summary>
    /// URL-decodes the text and removes path separator characters.
    /// </summary>
    /// <param name="text">The URL-encoded text to decode and clean.</param>
    /// <returns>The decoded and cleaned text.</returns>
    public static string UrlDecodeWithRemovePathSeparatorCharacter(string text)
    {
        text = WebUtility.UrlDecode(text);
        text = SHReplace.ReplaceAll(text, "", "%22", "%5c");
        return text;
    }

    /// <summary>
    /// Changes the extension of a URI path.
    /// </summary>
    /// <param name="text">The URI path to modify.</param>
    /// <param name="oldExtension">The current extension to remove.</param>
    /// <param name="newExtension">The new extension to append.</param>
    /// <returns>The URI path with the changed extension.</returns>
    public static string ChangeExtension(string text, string oldExtension, string newExtension)
    {
        text = SHTrim.TrimEnd(text, oldExtension);
        return text + newExtension;
    }

    /// <summary>
    /// Combines URI segments, trimming trailing slashes from each segment.
    /// </summary>
    /// <param name="segments">The URI segments to combine.</param>
    /// <returns>The combined URI without a trailing slash.</returns>
    public static string CombineTrimEndSlash(params string[] segments)
    {
        var result = new StringBuilder();
        foreach (var item in segments)
        {
            if (string.IsNullOrWhiteSpace(item))
                continue;
            if (item[item.Length - 1] == '/')
                result.Append(item.TrimStart('/'));
            else
                result.Append(item.TrimStart('/') + '/');
        }

        return result.ToString().TrimEnd('/');
    }

    /// <summary>
    /// URL-encodes the text after trimming whitespace.
    /// </summary>
    /// <param name="text">The text to encode.</param>
    /// <returns>The URL-encoded text.</returns>
    public static string UrlEncode(string text)
    {
        return WebUtility.UrlEncode(text.Trim());
    }

    /// <summary>
    /// URL-decodes the text after trimming whitespace.
    /// </summary>
    /// <param name="text">The text to decode.</param>
    /// <returns>The URL-decoded text.</returns>
    public static string UrlDecode(string text)
    {
        return WebUtility.UrlDecode(text.Trim());
    }

    /// <summary>
    /// Gets the file name from a URI, optionally returning the full URL without query string.
    /// </summary>
    /// <param name="text">The URI to extract the file name from.</param>
    /// <param name="isReturningWholeUrl">If true, returns the full URL without query string instead of just the file name.</param>
    /// <returns>The file name or full URL without query string.</returns>
    public static string GetFileName(string text, bool isReturningWholeUrl = false)
    {
        if (isReturningWholeUrl)
        {
            var data = SHParts.RemoveAfterFirst(text, "?");
            return data;
        }

        text = SHParts.RemoveAfterFirst(text, "?");
        text = text.TrimEnd('/');
        var lastSlashIndex = text.LastIndexOf('/');
        return text.Substring(lastSlashIndex + 1);
    }

    /// <summary>
    /// Gets the file extension from a URI.
    /// </summary>
    /// <param name="text">The URI to extract the extension from.</param>
    /// <returns>The file extension including the leading dot.</returns>
    public static string GetExtension(string text)
    {
        return Path.GetExtension(text);
    }
}
