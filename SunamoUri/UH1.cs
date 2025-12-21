namespace SunamoUri;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class UH
{
    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon =>
    ///     ?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon
    ///     Vr�t� cel� QS value�etn� po��te�n�ho otazn�ku
    /// </summary>
    public static string GetQueryAsHttpRequest(Uri uri)
    {
        return uri.Query;
    }

    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => /Me/Login.aspx
    /// </summary>
    public static string GetPageNameFromUri(Uri uri)
    {
        var nt = uri.PathAndQuery.IndexOf("?");
        if (nt != -1)
            return uri.PathAndQuery.Substring(0, nt);
        return uri.PathAndQuery;
    }

    ///// <summary>
    ///// https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => GetPageNameFromUriTest: /Me/Login.aspx
    /////
    ///// Nonsense - Join A1,2 to return back A1
    ///// </summary>
    //public static string GetPageNameFromUri(string atr, string host)
    //{
    //    if (!atr.StartsWith("https://") && !atr.StartsWith("https://"))
    //    {
    //        return GetPageNameFromUri(new Uri("https://" + host + "/" + atr.TrimStart('/')));
    //    }
    //    return GetPageNameFromUri(new Uri(atr));
    //}
    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon =>
    ///     GetFileNameWithoutExtension: Login
    ///     Pod�v� naprosto stejn� value�sledek jako UH.GetPageNameFromUri
    /// </summary>
    /// <param name = "uri"></param>
    public static string GetFilePathAsHttpRequest(Uri uri)
    {
        return uri.LocalPath;
    }

    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon =>
    /// </summary>
    public static string GetProtocolString(Uri uri)
    {
        return uri.Scheme + "://";
    }

    /// <summary>
    ///     Vr�t� true pokud m� A1 protokol http nebo https
    /// </summary>
    /// <param name = "p"></param>
    public static bool HasHttpProtocol(string parameter)
    {
        parameter = parameter.ToLower();
        if (parameter.StartsWith("http://"))
            return true;
        if (parameter.StartsWith("https://"))
            return true;
        return false;
    }

    /// <summary>
    ///     create also for page:
    /// </summary>
    /// <param name = "s"></param>
    /// <returns></returns>
    public static Uri CreateUri(ILogger logger, string text)
    {
        try
        {
            return new Uri(text);
        }
        catch (Exception ex)
        {
            logger.LogError("Can't construct url from " + text);
            //ThrowEx.Custom(ex);
            return null;
        }
    }

    public static string urlDecoded;
    public static bool IsUrlEncoded(string uri)
    {
        urlDecoded = UrlDecode(uri);
        return urlDecoded != uri;
    }

    public static string HostUriToPascalConvention(ILogger logger, string text)
    {
        // Uri must be checked always before passed into method. Then I would make same checks again and again
        var uri = CreateUri(logger, text);
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
        // remove entities
        title = Regex.Replace(title, @"&\w+;", "");
        // remove anything that is not letters, numbers, dash, or space
        title = Regex.Replace(title, @"[^A-Za-z0-9\-\s]", "");
        // remove any leading or trailing spaces left over
        title = title.Trim();
        // replace spaces with single dash
        title = Regex.Replace(title, @"\s+", "-");
        // if we end up with multiple dashes, collapse to single dash
        title = Regex.Replace(title, @"\-{2,}", "-");
        // make it all lower case
        title = title.ToLower();
        // if it's too long, clip it
        if (title.Length > 80)
            title = title.Substring(0, 79);
        // remove trailing dash, if there is one
        if (title.EndsWith("-"))
            title = title.Substring(0, title.Length - 1);
        return title;
    }

    public static string InsertBetweenPathAndFile(string uri, string vlozit)
    {
        var text = uri.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        text[text.Count - 2] += "/" + vlozit;
        //Uri uri2 = new Uri(uri);
        string vr = null;
        vr = Join(text.ToArray());
        return vr.Replace(":/", "://");
    }

    public static bool Contains(ILogger logger, Uri source, string hostnameEndsWith, string pathContaint, params string[] qsContainsAll)
    {
        hostnameEndsWith = hostnameEndsWith.ToLower();
        pathContaint = pathContaint.ToLower();
        var uri = CreateUri(logger, source.ToString().ToLower());
        if (uri.Host.EndsWith(hostnameEndsWith))
            if (GetFilePathAsHttpRequest(uri).Contains(pathContaint))
                foreach (var item in qsContainsAll)
                {
                    if (!uri.Query.Contains(item))
                        return false;
                    return true;
                }

        return false;
    }

    public static bool IsHttpDecoded(ref string input)
    {
        var decoded = WebUtility.UrlDecode(input);
        if (true)
        {
        }

        return false;
    }

    public static string RemoveTrackingPart(string value)
    {
        var result = SHParts.RemoveAfterFirst(value, "#utm_");
        result = RemovePrefixHttpOrHttps(result);
        result = SHParts.RemoveAfterFirstChar(result, '/');
        if (result.Contains("."))
            return "https://" + result;
        return result;
    //return value.Substring("#utm_source")
    }

    /// <summary>
    ///     A2 can be * - then return true for any domain
    /// </summary>
    /// <param name = "p"></param>
    /// <param name = "domain"></param>
    public static bool IsValidUriAndDomainIs(string parameter, string domain, out bool surelyDomain)
    {
        var p2 = AppendHttpIfNotExists(parameter);
        Uri uri = null;
        surelyDomain = false;
        // Nema smysl hledat na přípony souborů, vrátil bych false pro to co by možná byla doména. Dnes už doména může být opravdu jakákoliv
        if (Uri.TryCreate(p2, UriKind.Absolute, out uri))
            if (uri.Host == domain || domain == "*")
                return true;
        return false;
    }

    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => lyrics.sunamo.cz
    /// </summary>
    public static string GetHost(ILogger logger, string text)
    {
        var u = CreateUri(logger, AppendHttpIfNotExists(text));
        return u.Host;
    }

    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon =>
    ///     https://lyrics.sunamo.cz/Me/
    ///     Return by convetion with / on end
    /// </summary>
    /// <param name = "rp"></param>
    public static string GetDirectoryName(string rp)
    {
        if (rp != "/")
            rp = rp.TrimEnd('/');
        rp = SHParts.RemoveAfterFirstChar(rp, '?');
        var dex = rp.LastIndexOf('/');
        if (dex != -1)
            return rp.Substring(0, dex + 1);
        return rp;
    }

    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => Login
    /// </summary>
    /// <param name = "p"></param>
    public static string GetFileNameWithoutExtension(string parameter)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(parameter));
    }

    /// <param name = "p"></param>
    public static string Combine(bool dir, params string[] parameter)
    {
        var vr = string.Join('/', parameter).Replace("///", "/").Replace("//", "/").TrimEnd('/').Replace(":/", "://");
        if (dir)
            vr += "/";
        return vr;
    }

    private static string Join(params string[] text)
    {
        return string.Join('/', text);
    }

    public static string Combine(params string[] parameter)
    {
        return Combine(parameter.ToList());
    }

    /// <param name = "p"></param>
    public static string Combine(IList<string> parameter)
    {
        var vr = new StringBuilder();
        var i = 0;
        foreach (var item in parameter)
        {
            i++;
            if (string.IsNullOrWhiteSpace(item))
                continue;
            if (item[item.Length - 1] == '/')
            {
                vr.Append(item);
            }
            else
            {
                if (i == parameter.Count && Path.GetExtension(item) != "")
                    vr.Append(item);
                else
                    vr.Append(item + '/');
            }
        //vr.Append(item.TrimEnd('/') + "/");
        }

        return vr.ToString();
    }
}