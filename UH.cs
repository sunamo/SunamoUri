
namespace SunamoUri;
using Case.NET;
using SunamoStringParts;



public partial class UH
{
    #region Other methods

    public static string HostUriToPascalConvention(string s)
    {
        // Uri must be checked always before passed into method. Then I would make same checks again and again
        var uri = CreateUri(s);
        var result = SHReplace.ReplaceAll(uri.Host, AllStrings.space, new string[] { AllStrings.dot });
        result = CaseConverter.CamelCase.ConvertCase(result);

        StringBuilder sb = new StringBuilder(result);
        sb[0] = char.ToUpper(sb[0]);
        return sb.ToString();

    }

    private static string GetUriSafeString2(string title)
    {
        if (String.IsNullOrEmpty(title)) return "";

        // remove entities
        title = Regex.Replace(title, @"&\w+;", "");
        // remove anything that is not letters, numbers, dash, or space
        title = Regex.Replace(title, @"[^A-Za-z0-9\-\s]", "");
        // remove any leading or trailing spaces left over
        title = title.Trim();
        // replace spaces with single dash
        title = Regex.Replace(title, @"\s+", AllStrings.dash);
        // if we end up with multiple dashes, collapse to single dash
        title = Regex.Replace(title, @"\-{2,}", AllStrings.dash);
        // make it all lower case
        title = title.ToLower();
        // if it's too long, clip it
        if (title.Length > 80)
            title = title.Substring(0, 79);
        // remove trailing dash, if there is one
        if (title.EndsWith(AllStrings.dash))
            title = title.Substring(0, title.Length - 1);
        return title;
    }

    public static string InsertBetweenPathAndFile(string uri, string vlozit)
    {
        var s = uri.Split(new String[] { AllStrings.slash }, StringSplitOptions.RemoveEmptyEntries).ToList();
        s[s.Count - 2] += AllStrings.slash + vlozit;
        //Uri uri2 = new Uri(uri);
        string vr = null;
        vr = Join(s.ToArray());
        return vr.Replace(":/", "://");
    }

    public static bool Contains(Uri source, string hostnameEndsWith, string pathContaint, params string[] qsContainsAll)
    {
        hostnameEndsWith = hostnameEndsWith.ToLower();
        pathContaint = pathContaint.ToLower();
        Uri uri = CreateUri(source.ToString().ToLower());
        if (uri.Host.EndsWith(hostnameEndsWith))
        {
            if (UH.GetFilePathAsHttpRequest(uri).Contains(pathContaint))
            {
                foreach (var item in qsContainsAll)
                {
                    if (!uri.Query.Contains(item))
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
        return false;
    }


    #endregion

    #region Is*
    public static bool IsHttpDecoded(ref string input)
    {
        string decoded = WebUtility.UrlDecode(input);
        if (true)
        {
        }
        return false;
    }

    public static string RemoveTrackingPart(string v)
    {
        var r = SHParts.RemoveAfterFirst(v, "#utm_");
        r = UH.RemovePrefixHttpOrHttps(r);
        r = SHParts.RemoveAfterFirstChar(r, AllChars.slash);

        if (r.Contains(AllStrings.dot))
        {
            return Consts.https + r;
        }

        return r;
        //return v.Substring("#utm_source")
    }

    /// <summary>
    /// A2 can be * - then return true for any domain
    /// </summary>
    /// <param name="p"></param>
    /// <param name="domain"></param>
    public static bool IsValidUriAndDomainIs(string p, string domain, out bool surelyDomain)
    {
        string p2 = AppendHttpIfNotExists(p);
        Uri uri = null;
        surelyDomain = false;

        // Nema smysl hledat na přípony souborů, vrátil bych false pro to co by možná byla doména. Dnes už doména může být opravdu jakákoliv

        if (Uri.TryCreate(p2, UriKind.Absolute, out uri))
        {
            if (uri.Host == domain || domain == "*")
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Get parts of URI
    /// <summary>
    /// https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => lyrics.sunamo.cz
    /// </summary>
    public static string GetHost(string s)
    {
        var u = CreateUri(AppendHttpIfNotExists(s));
        return u.Host;
    }

    /// <summary>
    /// https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => https://lyrics.sunamo.cz/Me/
    /// Return by convetion with / on end
    /// </summary>
    /// <param name="rp"></param>
    public static string GetDirectoryName(string rp)
    {
        if (rp != AllStrings.slash)
        {
            rp = rp.TrimEnd(AllChars.slash);
        }

        rp = SHParts.RemoveAfterFirstChar(rp, AllChars.q);

        int dex = rp.LastIndexOf(AllChars.slash);
        if (dex != -1)
        {
            return rp.Substring(0, dex + 1);
        }
        return rp;
    }

    /// <summary>
    /// https://lyrics.sunamo.cz/Me/Login.aspx?ReturnUrl=https://lyrics.sunamo.cz/Artist/walk-the-moon => Login
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
        string vr = string.Join(AllChars.slash, p).Replace("///", AllStrings.slash).Replace("//", AllStrings.slash).TrimEnd(AllChars.slash).Replace(":/", "://");
        if (dir)
        {
            vr += AllStrings.slash;
        }
        return vr;
    }

    private static string Join(params string[] s)
    {
        return string.Join(AllChars.slash, s);
    }

    public static string Combine(params string[] p)
    {
        return Combine(p.ToList());
    }

    /// <param name="p"></param>
    public static string Combine(IList<string> p)
    {
        StringBuilder vr = new StringBuilder();
        int i = 0;
        foreach (string item in p)
        {
            i++;
            if (string.IsNullOrWhiteSpace(item))
            {
                continue;
            }
            if (item[item.Length - 1] == AllChars.slash)
            {
                vr.Append(item);
            }
            else
            {
                if (i == p.Count && Path.GetExtension(item) != "")
                {
                    vr.Append(item);
                }
                else
                {
                    vr.Append(item + AllChars.slash);
                }
            }
            //vr.Append(item.TrimEnd(AllChars.slash) + AllStrings.slash);
        }
        return vr.ToString();
    }
    #endregion

    #region Ŕemove*


    /// <summary>
    /// V p��pad� �e v A1 nebude protokol, ulo�� se do A2 ""
    /// V p��pad� �e tam protokol bude, ulo�� se do A2 v�etn� ://
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
    /// pass also for page:
    /// </summary>
    /// <param name="href"></param>
    /// <returns></returns>
    public static bool IsUri(string href)
    {
        var uri = CreateUri(href);
        return uri != null;
    }
    #endregion

    /// <summary>
    /// first upper, other lower
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static string DebugLocalhost(string v)
    {
        v = v.ToLower();
        StringBuilder sb = new StringBuilder(v);
        sb[0] = char.ToUpper(sb[0]);
        v = sb.ToString();

        if (v != sess.i18n(XlfKeys.Nope))
        {
            List<FieldInfo> co = null;

            co = typeof(UriShortConsts).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
            var co2 = co.Where(d => d.Name.StartsWith(v)).First();
            var vr = Consts.https + co2.GetValue(null).ToString() + "/";
            return vr;
        }
#if !DEBUG

        return Consts.HttpSunamoCzSlash;
#endif
        return Consts.HttpLocalhostSlash;
    }

    public static bool IsWellFormedUriString(ref string uri, UriKind absolute)
    {
        uri = uri.Trim();
        uri = uri.TrimEnd(AllChars.colon);

        var v = Uri.IsWellFormedUriString(uri, absolute);
        if (v)
        {
            uri = UH.AppendHttpIfNotExists(uri);
        }
        return v;
    }

    public static string GetPathname(string uri)
    {
        uri = UH.RemovePrefixHttpOrHttps(uri);

        uri = SHParts.KeepAfterFirst(uri, AllStrings.slash, false);

        return uri;
    }

    public static string SanitizeKeepOnlyHost(string s)
    {
        s = UH.RemoveProtocol(s);
        s = SHParts.RemoveAfterFirstChar(s, AllChars.slash);
        s = s.Replace("www.", "");
        s = s.TrimEnd(AllChars.slash);

        return s;
    }

    private static string RemoveProtocol(string s)
    {
        s = SunamoStringShared.SHSH.ReplaceOnce(s, Consts.http, Consts.se);
        s = SunamoStringShared.SHSH.ReplaceOnce(s, Consts.https, Consts.se);

        return s;
    }

    /// <summary>
    /// Remove also last slash and www.
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static string KeepOnlyHostAndProtocol(string v)
    {
        var p = v.Split(new String[] { Consts.lc }, StringSplitOptions.RemoveEmptyEntries).ToList(); //SHSplit.Split(v, );

        // se to tu už může dostat bez protokolu
        //if (p.Count != 2)
        //{
        //    throw new Exception("Wrong count of parts");
        //}

        var dx = 0;
        if (p.Count == 2)
        {
            dx = 1;
        }

        p[dx] = SHParts.RemoveAfterFirstChar(p[dx], AllChars.slash);

        return SHTrim.TrimStart(string.Join(Consts.lc, p).TrimEnd(AllChars.slash), "www.");
    }

    public static string GetToken(string href, int v)
    {
        var tokens = href.Split(new String[] { AllStrings.slash }, StringSplitOptions.RemoveEmptyEntries).ToList(); //SHSplit.Split(href, AllStrings.slash);

        return tokens[tokens.Count + v];
    }
}
