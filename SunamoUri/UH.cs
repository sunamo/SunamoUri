namespace SunamoUri;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class UH
{
    public static string RemoveLastChar(string artist)
    {
        return artist.Substring(0, artist.Length - 1);
    }

    public static string WhiteSpaceFromStart(string value)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in value)
            if (char.IsWhiteSpace(item))
                stringBuilder.Append(item);
            else
                break;
        return stringBuilder.ToString();
    }

    public static string RemoveHostAndProtocol(Uri uri)
    {
        var parameter = RemovePrefixHttpOrHttps(uri.ToString());
        var dex = parameter.IndexOf('/');
        return parameter.Substring(dex);
    }

    public static string RemovePrefixHttpOrHttps(string t)
    {
        t = t.Replace("http://", "");
        t = t.Replace("https://", "");
        return t;
    }

    /// <summary>
    ///     first upper, other lower
    /// </summary>
    /// <param name = "v"></param>
    /// <returns></returns>
    public static string DebugLocalhost(string value)
    {
        value = value.ToLower();
        var stringBuilder = new StringBuilder(value);
        stringBuilder[0] = char.ToUpper(stringBuilder[0]);
        value = stringBuilder.ToString();
        if (value != Translate.FromKey(XlfKeys.Nope))
        {
            List<FieldInfo> co = null;
            co = typeof(UriShortConsts).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
            var co2 = co.Where(data => data.Name.StartsWith(value)).First();
            var vr = "https://" + co2.GetValue(null) + " / ";
            return vr;
        }

#if !DEBUG
        return "https://sunamo.cz";
#endif
        return "https://sunamo.net";
    }

    public static bool IsWellFormedUriString(ref string uri, UriKind absolute)
    {
        uri = uri.Trim();
        uri = uri.TrimEnd(':');
        var value = Uri.IsWellFormedUriString(uri, absolute);
        if (value)
            uri = AppendHttpIfNotExists(uri);
        return value;
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

    /// <summary>
    ///     Remove also last slash and www.
    /// </summary>
    /// <param name = "v"></param>
    /// <returns></returns>
    public static string KeepOnlyHostAndProtocol(string value)
    {
        var parameter = value.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries).ToList(); //SHSplit.Split(value, );
        // se to tu už může dostat bez protokolu
        //if (parameter.Count != 2)
        //{
        //    throw new Exception("Wrong count of parts");
        //}
        var dx = 0;
        if (parameter.Count == 2)
            dx = 1;
        parameter[dx] = SHParts.RemoveAfterFirstChar(parameter[dx], '/');
        return SHTrim.TrimStart(string.Join("//", parameter).TrimEnd('/'), "www.");
    }

    public static string GetToken(string href, int value)
    {
        var tokens = href.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList(); //SHSplit.Split(href, "/");
        return tokens[tokens.Count + value];
    }

    public static string AppendHttpsIfNotExists(string parameter)
    {
        var p2 = parameter;
        if (!parameter.StartsWith("https"))
            p2 = "https://" + parameter;
        return p2;
    }

    public static string AppendHttpIfNotExists(string parameter)
    {
        var p2 = parameter;
        if (!parameter.StartsWith("http"))
            p2 = "http://" + parameter;
        return p2;
    }

    public static string GetUriSafeString(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "";
        title = SH.AddBeforeUpperChars(title, '-', false);
        title = title.RemoveDiacritics();
        // replace spaces with single dash
        title = Regex.Replace(title, @"\s+", "-");
        // if we end up with multiple dashes, collapse to single dash
        title = Regex.Replace(title, @"\-{2,}", "-");
        // make it all lower case
        title = title.ToLower();
        // remove entities
        title = Regex.Replace(title, @"&\w+;", "");
        // remove anything that is not letters, numbers, dash, or space
        title = Regex.Replace(title, @"[^a-z0-9\-\s]", "");
        // replace spaces
        title = title.Replace(' ', '-');
        // collapse dashes
        title = Regex.Replace(title, @"-{2,}", "-");
        // trim excessive dashes at the beginning
        title = title.TrimStart(new[] { '-' });
        // if it's too long, clip it
        if (title.Length > 80)
            title = title.Substring(0, 79);
        // remove trailing dashes
        title = title.TrimEnd(new[] { '-' });
        return title;
    }

    public static void BeforeCombine(ref string hostApp)
    {
        hostApp = SH.PrefixIfNotStartedWith(hostApp, "https://");
        hostApp = SH.PostfixIfNotEmpty(hostApp, "/");
    }

    public static string GetUriSafeString(string title, int maxLenght)
    {
        if (string.IsNullOrEmpty(title))
            return "";
        title = title.RemoveDiacritics();
        // replace spaces with single dash
        title = Regex.Replace(title, @"\s+", "-");
        // if we end up with multiple dashes, collapse to single dash
        title = Regex.Replace(title, @"\-{2,}", "-");
        // make it all lower case
        title = title.ToLower();
        // remove entities
        title = Regex.Replace(title, @"&\w+;", "");
        // remove anything that is not letters, numbers, dash, or space
        title = Regex.Replace(title, @"[^a-z0-9\-\s]", "");
        // replace spaces
        title = title.Replace(' ', '-');
        // collapse dashes
        title = Regex.Replace(title, @"-{2,}", "-");
        // trim excessive dashes at the beginning
        title = title.TrimStart(new[] { '-' });
        // remove trailing dashes
        title = title.TrimEnd(new[] { '-' });
        title = SHReplace.ReplaceAll(title, "-", "--");
        // if it's too long, clip it
        if (title.Length > maxLenght)
            title = title.Substring(0, maxLenght);
        return title;
    }

    public static string GetUriSafeString(string tagName, int maxLength, Func<string, bool> methodInWebExists)
    {
        var uri = GetUriSafeString(tagName, maxLength);
        var pripocist = 1;
        while (methodInWebExists.Invoke(uri))
            if (uri.Length + pripocist.ToString().Length >= maxLength)
            {
                tagName = tagName.Substring(0, tagName.Length - 1); //SH.RemoveLastChar(tagName);
            }
            else
            {
                var prip = pripocist.ToString();
                if (pripocist == 1)
                    prip = "";
                uri = GetUriSafeString(tagName + prip, maxLength);
                pripocist++;
            }

        return uri;
    }

    public static string UrlDecodeWithRemovePathSeparatorCharacter(string pridat)
    {
        pridat = WebUtility.UrlDecode(pridat);
        //%22 = \
        pridat = SHReplace.ReplaceAll(pridat, "", "%22", "%5c");
        return pridat;
    }

    public static string ChangeExtension(string attrA, string oldExt, string extL)
    {
        attrA = SHTrim.TrimEnd(attrA, oldExt);
        return attrA + extL;
    }

    public static string CombineTrimEndSlash(params string[] parameter)
    {
        var vr = new StringBuilder();
        foreach (var item in parameter)
        {
            if (string.IsNullOrWhiteSpace(item))
                continue;
            if (item[item.Length - 1] == '/')
                vr.Append(item.TrimStart('/'));
            else
                vr.Append(item.TrimStart('/') + '/');
        //vr.Append(item.TrimEnd('/') + "/");
        }

        return vr.ToString().TrimEnd('/');
    }

    public static string UrlEncode(string co)
    {
        return WebUtility.UrlEncode(co.Trim());
    }

    public static string UrlDecode(string co)
    {
        return WebUtility.UrlDecode(co.Trim());
    }

    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => Login.aspx
    /// </summary>
    /// <param name = "rp"></param>
    public static string GetFileName(string rp, bool wholeUrl = false)
    {
        if (wholeUrl)
        {
            var data = SHParts.RemoveAfterFirst(rp, "?");
            //var result = FS.ReplaceInvalidFileNameChars(data, []);
            return data;
        }

        rp = SHParts.RemoveAfterFirst(rp, "?");
        rp = rp.TrimEnd('/');
        var dex = rp.LastIndexOf('/');
        return rp.Substring(dex + 1);
    }

    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => ""
    /// </summary>
    public static string GetExtension(string image)
    {
        return Path.GetExtension(image);
    }
}