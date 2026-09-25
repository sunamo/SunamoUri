namespace SunamoUri;

public partial class UH
{
    public static string RemoveLastChar(string text)
    {
        return text.Substring(0, text.Length - 1);
    }

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

    public static string RemoveHostAndProtocol(Uri uri)
    {
        var textWithoutProtocol = RemovePrefixHttpOrHttps(uri.ToString());
        var slashIndex = textWithoutProtocol.IndexOf('/');
        return textWithoutProtocol.Substring(slashIndex);
    }

    public static string RemovePrefixHttpOrHttps(string text)
    {
        text = text.Replace("http://", "");
        text = text.Replace("https://", "");
        return text;
    }

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

        return "https://sunamo.net";
    }

    public static bool IsWellFormedUriString(ref string uri, UriKind uriKind)
    {
        uri = uri.Trim();
        uri = uri.TrimEnd(':');
        var isWellFormed = Uri.IsWellFormedUriString(uri, uriKind);
        if (isWellFormed)
            uri = AppendHttpIfNotExists(uri);
        return isWellFormed;
    }

    public static string GetPathname(string uri)
    {
        uri = RemovePrefixHttpOrHttps(uri);
        uri = SHParts.KeepAfterFirst(uri, "/");
        return uri;
    }

    public static string SanitizeKeepOnlyHost(string text)
    {
        text = RemoveProtocol(text);
        text = SHParts.RemoveAfterFirstChar(text, '/');
        text = text.Replace("www.", "");
        text = text.TrimEnd('/');
        return text;
    }

    private static string RemoveProtocol(string text)
    {
        text = SH.ReplaceOnce(text, "http:", "");
        text = SH.ReplaceOnce(text, "https:", "");
        return text;
    }

    public static string KeepOnlyHostAndProtocol(string text)
    {
        var parts = text.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var index = 0;
        if (parts.Count == 2)
            index = 1;
        parts[index] = SHParts.RemoveAfterFirstChar(parts[index], '/');
        return SHTrim.TrimStart(string.Join("//", parts).TrimEnd('/'), "www.");
    }

    public static string GetToken(string text, int offset)
    {
        var parts = text.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        return parts[parts.Count + offset];
    }

    public static string AppendHttpsIfNotExists(string text)
    {
        var result = text;
        if (!text.StartsWith("https"))
            result = "https://" + text;
        return result;
    }

    public static string AppendHttpIfNotExists(string text)
    {
        var result = text;
        if (!text.StartsWith("http"))
            result = "http://" + text;
        return result;
    }

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

    public static void BeforeCombine(ref string hostApp)
    {
        hostApp = SH.PrefixIfNotStartedWith(hostApp, "https://");
        hostApp = SH.PostfixIfNotEmpty(hostApp, "/");
    }

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

    public static string UrlDecodeWithRemovePathSeparatorCharacter(string text)
    {
        text = WebUtility.UrlDecode(text);
        text = SHReplace.ReplaceAll(text, "", "%22", "%5c");
        return text;
    }

    public static string ChangeExtension(string text, string oldExtension, string newExtension)
    {
        text = SHTrim.TrimEnd(text, oldExtension);
        return text + newExtension;
    }

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

    public static string UrlEncode(string text)
    {
        return WebUtility.UrlEncode(text.Trim());
    }

    public static string UrlDecode(string text)
    {
        return WebUtility.UrlDecode(text.Trim());
    }

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

    public static string GetExtension(string text)
    {
        return Path.GetExtension(text);
    }
}
