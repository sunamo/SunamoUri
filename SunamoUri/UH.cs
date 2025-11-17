// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoUri;

public class UH
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
    #region Remove*
    public static string RemovePrefixHttpOrHttps(string t)
    {
        t = t.Replace("http://", "");
        t = t.Replace("https://", "");
        return t;
    }
    #endregion
    /// <summary>
    ///     first upper, other lower
    /// </summary>
    /// <param name="v"></param>
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
            co = typeof(UriShortConsts)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
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
        if (value) uri = AppendHttpIfNotExists(uri);
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
    /// <param name="v"></param>
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
        if (parameter.Count == 2) dx = 1;
        parameter[dx] = SHParts.RemoveAfterFirstChar(parameter[dx], '/');
        return SHTrim.TrimStart(string.Join("//", parameter).TrimEnd('/'), "www.");
    }
    public static string GetToken(string href, int value)
    {
        var tokens = href.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
            .ToList(); //SHSplit.Split(href, "/");
        return tokens[tokens.Count + value];
    }
    #region Append
    public static string AppendHttpsIfNotExists(string parameter)
    {
        var p2 = parameter;
        if (!parameter.StartsWith("https")) p2 = "https://" + parameter;
        return p2;
    }
    public static string AppendHttpIfNotExists(string parameter)
    {
        var p2 = parameter;
        if (!parameter.StartsWith("http")) p2 = "http://" + parameter;
        return p2;
    }
    #endregion
    #region GetUriSafeString
    public static string GetUriSafeString(string title)
    {
        if (string.IsNullOrEmpty(title)) return "";
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
        if (string.IsNullOrEmpty(title)) return "";
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
                if (pripocist == 1) prip = "";
                uri = GetUriSafeString(tagName + prip, maxLength);
                pripocist++;
            }
        return uri;
    }
    #endregion
    #region Change in uri
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
            if (string.IsNullOrWhiteSpace(item)) continue;
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
    /// <param name="rp"></param>
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
    #endregion
    #region Get parts of uri
    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => ""
    /// </summary>
    public static string GetExtension(string image)
    {
        return Path.GetExtension(image);
    }
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
        if (nt != -1) return uri.PathAndQuery.Substring(0, nt);
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
    /// <param name="uri"></param>
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
    #endregion
    #region Other
    /// <summary>
    ///     Vr�t� true pokud m� A1 protokol http nebo https
    /// </summary>
    /// <param name="p"></param>
    public static bool HasHttpProtocol(string parameter)
    {
        parameter = parameter.ToLower();
        if (parameter.StartsWith("http://")) return true;
        if (parameter.StartsWith("https://")) return true;
        return false;
    }
    /// <summary>
    ///     create also for page:
    /// </summary>
    /// <param name="s"></param>
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
    #endregion
    #region Other methods
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
        if (string.IsNullOrEmpty(title)) return "";
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
                    if (!uri.Query.Contains(item)) return false;
                    return true;
                }
        return false;
    }
    #endregion
    #region Is*
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
        if (result.Contains(".")) return "https://" + result;
        return result;
        //return value.Substring("#utm_source")
    }
    /// <summary>
    ///     A2 can be * - then return true for any domain
    /// </summary>
    /// <param name="p"></param>
    /// <param name="domain"></param>
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
    #endregion
    #region Get parts of URI
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
    /// <param name="rp"></param>
    public static string GetDirectoryName(string rp)
    {
        if (rp != "/") rp = rp.TrimEnd('/');
        rp = SHParts.RemoveAfterFirstChar(rp, '?');
        var dex = rp.LastIndexOf('/');
        if (dex != -1) return rp.Substring(0, dex + 1);
        return rp;
    }
    /// <summary>
    ///     https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => Login
    /// </summary>
    /// <param name="p"></param>
    public static string GetFileNameWithoutExtension(string parameter)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(parameter));
    }
    #endregion
    #region Join, Combine
    /// <param name="p"></param>
    public static string Combine(bool dir, params string[] parameter)
    {
        var vr = string.Join('/', parameter).Replace("///", "/").Replace("//", "/")
            .TrimEnd('/').Replace(":/", "://");
        if (dir) vr += "/";
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
    /// <param name="p"></param>
    public static string Combine(IList<string> parameter)
    {
        var vr = new StringBuilder();
        var i = 0;
        foreach (var item in parameter)
        {
            i++;
            if (string.IsNullOrWhiteSpace(item)) continue;
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
    #endregion
    #region Ŕemove*
    /// <summary>
    ///     value parameter��pad� �e value A1 nebude protokol, ulo�� se do A2 ""
    ///     value parameter��pad� �e tam protokol bude, ulo�� se do A2 value�etn� ://
    /// </summary>
    /// <param name="t"></param>
    /// <param name="protocol"></param>
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
    /// <param name="href"></param>
    /// <returns></returns>
    public static bool IsUri(ILogger logger, string href)
    {
        var uri = CreateUri(logger, href);
        return uri != null;
    }
    #endregion
}