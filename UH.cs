namespace SunamoUri;

public class UH
{
    public static string RemoveLastChar(string artist)
    {
        return artist.Substring(0, artist.Length - 1);
    }
    public static string WhiteSpaceFromStart(string v)
    {
        var sb = new StringBuilder();
        foreach (var item in v)
            if (char.IsWhiteSpace(item))
                sb.Append(item);
            else
                break;
        return sb.ToString();
    }
    public static string RemoveHostAndProtocol(Uri uri)
    {
        var p = RemovePrefixHttpOrHttps(uri.ToString());
        var dex = p.IndexOf('/');
        return p.Substring(dex);
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
    public static string DebugLocalhost(string v)
    {
        v = v.ToLower();
        var sb = new StringBuilder(v);
        sb[0] = char.ToUpper(sb[0]);
        v = sb.ToString();
        if (v != Translate.FromKey(XlfKeys.Nope))
        {
            List<FieldInfo> co = null;
            co = typeof(UriShortConsts)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
            var co2 = co.Where(d => d.Name.StartsWith(v)).First();
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
        var v = Uri.IsWellFormedUriString(uri, absolute);
        if (v) uri = AppendHttpIfNotExists(uri);
        return v;
    }
    public static string GetPathname(string uri)
    {
        uri = RemovePrefixHttpOrHttps(uri);
        uri = SHParts.KeepAfterFirst(uri, "/");
        return uri;
    }
    public static string SanitizeKeepOnlyHost(string s)
    {
        s = RemoveProtocol(s);
        s = SHParts.RemoveAfterFirstChar(s, '/');
        s = s.Replace("www.", "");
        s = s.TrimEnd('/');
        return s;
    }
    private static string RemoveProtocol(string s)
    {
        s = SH.ReplaceOnce(s, "http:", "");
        s = SH.ReplaceOnce(s, "https:", "");
        return s;
    }
    /// <summary>
    ///     Remove also last slash and www.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static string KeepOnlyHostAndProtocol(string v)
    {
        var p = v.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries).ToList(); //SHSplit.SplitMore(v, );
        // se to tu už může dostat bez protokolu
        //if (p.Count != 2)
        //{
        //    throw new Exception("Wrong count of parts");
        //}
        var dx = 0;
        if (p.Count == 2) dx = 1;
        p[dx] = SHParts.RemoveAfterFirstChar(p[dx], '/');
        return SHTrim.TrimStart(string.Join("//", p).TrimEnd('/'), "www.");
    }
    public static string GetToken(string href, int v)
    {
        var tokens = href.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
            .ToList(); //SHSplit.SplitMore(href, "/");
        return tokens[tokens.Count + v];
    }
    #region Append
    public static string AppendHttpsIfNotExists(string p)
    {
        var p2 = p;
        if (!p.StartsWith("https")) p2 = "https://" + p;
        return p2;
    }
    public static string AppendHttpIfNotExists(string p)
    {
        var p2 = p;
        if (!p.StartsWith("http")) p2 = "http://" + p;
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
    public static string CombineTrimEndSlash(params string[] p)
    {
        var vr = new StringBuilder();
        foreach (var item in p)
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
            var d = SHParts.RemoveAfterFirst(rp, "?");
            //var result = FS.ReplaceInvalidFileNameChars(d, []);
            return d;
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
    ///     Vr�t� cel� QS v�etn� po��te�n�ho otazn�ku
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
    ///     Pod�v� naprosto stejn� v�sledek jako UH.GetPageNameFromUri
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
    public static bool HasHttpProtocol(string p)
    {
        p = p.ToLower();
        if (p.StartsWith("http://")) return true;
        if (p.StartsWith("https://")) return true;
        return false;
    }
    /// <summary>
    ///     create also for page:
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static Uri CreateUri(string s)
    {
        try
        {
            return new Uri(s);
        }
        catch (Exception ex)
        {
            ThrowEx.Custom(ex);
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
    public static string HostUriToPascalConvention(string s)
    {
        // Uri must be checked always before passed into method. Then I would make same checks again and again
        var uri = CreateUri(s);
        var result = SHReplace.ReplaceAll(uri.Host, " ", ".");
        result = CaseConverter.CamelCase.ConvertCase(result);
        var sb = new StringBuilder(result);
        sb[0] = char.ToUpper(sb[0]);
        return sb.ToString();
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
        var s = uri.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        s[s.Count - 2] += "/" + vlozit;
        //Uri uri2 = new Uri(uri);
        string vr = null;
        vr = Join(s.ToArray());
        return vr.Replace(":/", "://");
    }
    public static bool Contains(Uri source, string hostnameEndsWith, string pathContaint, params string[] qsContainsAll)
    {
        hostnameEndsWith = hostnameEndsWith.ToLower();
        pathContaint = pathContaint.ToLower();
        var uri = CreateUri(source.ToString().ToLower());
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
    public static string RemoveTrackingPart(string v)
    {
        var r = SHParts.RemoveAfterFirst(v, "#utm_");
        r = RemovePrefixHttpOrHttps(r);
        r = SHParts.RemoveAfterFirstChar(r, '/');
        if (r.Contains(".")) return "https://" + r;
        return r;
        //return v.Substring("#utm_source")
    }
    /// <summary>
    ///     A2 can be * - then return true for any domain
    /// </summary>
    /// <param name="p"></param>
    /// <param name="domain"></param>
    public static bool IsValidUriAndDomainIs(string p, string domain, out bool surelyDomain)
    {
        var p2 = AppendHttpIfNotExists(p);
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
    public static string GetHost(string s)
    {
        var u = CreateUri(AppendHttpIfNotExists(s));
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
    public static string GetFileNameWithoutExtension(string p)
    {
        return Path.GetFileNameWithoutExtension(GetFileName(p));
    }
    #endregion
    #region Join, Combine
    /// <param name="p"></param>
    public static string Combine(bool dir, params string[] p)
    {
        var vr = string.Join('/', p).Replace("///", "/").Replace("//", "/")
            .TrimEnd('/').Replace(":/", "://");
        if (dir) vr += "/";
        return vr;
    }
    private static string Join(params string[] s)
    {
        return string.Join('/', s);
    }
    public static string Combine(params string[] p)
    {
        return Combine(p.ToList());
    }
    /// <param name="p"></param>
    public static string Combine(IList<string> p)
    {
        var vr = new StringBuilder();
        var i = 0;
        foreach (var item in p)
        {
            i++;
            if (string.IsNullOrWhiteSpace(item)) continue;
            if (item[item.Length - 1] == '/')
            {
                vr.Append(item);
            }
            else
            {
                if (i == p.Count && Path.GetExtension(item) != "")
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
    ///     V p��pad� �e v A1 nebude protokol, ulo�� se do A2 ""
    ///     V p��pad� �e tam protokol bude, ulo�� se do A2 v�etn� ://
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
    public static bool IsUri(string href)
    {
        var uri = CreateUri(href);
        return uri != null;
    }
    #endregion
}