using SunamoStringSplit;
using SunamoStringSubstring;

namespace SunamoUri;




/// <summary>
/// Summary description for QSHelper
/// </summary>
public partial class QSHelper
{
    /// <summary>
    /// GetParameter = return null when not found
    /// GetParameterSE = return string.Empty when not found
    /// </summary>
    public static string GetParameter(string uri, string nameParam)
    {
        var main = SHSplit.SplitMore(uri, new string[] { AllStringsSE.q, "&" });
        foreach (string var in main)
        {
            var v = SHSplit.Split(var, "=");
            if (v[0] == nameParam)
            {
                return v[1];
            }
        }

        return null;
    }

    public static string RemoveQs(string v)
    {
        var x = v.IndexOf('?');
        if (x != -1)
        {
            return v.Substring(0, x);
        }
        return v;
        //
    }

    /// <summary>
    /// get value of A2 parametr in A1
    /// GetParameter = return null when not found
    /// GetParameterSE = return string.Empty when not found
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="nameParam"></param>
    public static string GetParameterSE(string uri, string nameParam)
    {
        nameParam = nameParam + "=";
        int dexPocatek = uri.IndexOf(nameParam);
        if (dexPocatek != -1)
        {
            int dexKonec = uri.IndexOf("&", dexPocatek);
            dexPocatek = dexPocatek + nameParam.Length;
            if (dexKonec != -1)
            {
                return SHSubstring.Substring(uri, dexPocatek, dexKonec, SubstringArgs.Instance);
            }

            return uri.Substring(dexPocatek);
        }

        return "";
    }

    /// <summary>
    /// A1 je adresa bez konzového otazníku
    /// Všechny parametry automaticky zakóduje metodou UH.UrlEncode
    /// </summary>
    /// <param name = "adresa"></param>
    /// <param name = "p"></param>
    public static string GetQS(string adresa, params string[] p2)
    {
        var p = CA.TwoDimensionParamsIntoOne(p2);

        StringBuilder sb = new StringBuilder();
        sb.Append(adresa + AllStringsSE.q);
        int to = p.Count / 2 * 2;
        for (int i = 0; i < p.Count; i++)
        {
            if (i == to)
            {
                break;
            }

            string k = p[i].ToString();
            string v = UH.UrlEncode(p[++i].ToString());
            sb.Append(k + "=" + v + "&");
        }

        return sb.ToString().TrimEnd('&');
    }

    public static string GetQS(string adresa, Dictionary<string, string> p2)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(adresa + AllStringsSE.q);

        foreach (var item in p2)
        {
            sb.Append(item.Key + AllStringsSE.equals + item.Value + AllStringsSE.amp);
        }

        return sb.ToString().TrimEnd('&');
    }

    /// <summary>
    /// Do A1 se zadává Request.Url.Query.Substring(1) neboli třeba pid=1&amp;aid=10
    /// </summary>
    /// <param name = "args"></param>
    public static string GetNormalizeQS(string args)
    {
        if (args.Length != 0)
        {
            if (args.Contains("contextkey=") || args.Contains("guid=") || args.Contains("SelectingPhotos="))
            {
                // Pouze uploaduji fotky pomocí AjaxControlToolkit, ¨přece nebudu každé odeslání fotky ukládat do DB
                return null;
            }

            //args = args.Substring(1);
            List<string> splited = new List<string>(SHSplit.SplitChar(args, '&'));
            splited.Sort();
            args = string.Join('&', splited.ToArray());
        }

        return args;
    }

    /// <summary>
    /// Must get just qs without uri => use UH.GetQueryAsHttpRequest before
    /// </summary>
    /// <param name="qs"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ParseQs(string qs)
    {
        Dictionary<string, string> d = new Dictionary<string, string>();

        qs = qs.TrimStart(AllCharsSE.q);

        var parts = SHSplit.SplitMore(qs, new string[] { "&", "=" });

        return DictionaryHelper.GetDictionaryByKeyValueInString<string>(parts);
    }

    public static void GetArray(List<string> p, StringBuilder sb, bool uvo)
    {
        sb.Append("new Array(");
        //int to = (p.Length / 2) * 2;
        int to = p.Count;
        if (p.Count == 1)
        {
            to = 1;
        }

        int to2 = to - 1;
        if (to2 == -1)
        {
            to2 = 0;
        }

        if (uvo)
        {
            for (int i = 0; i < to; i++)
            {
                string k = p[i].ToString();
                sb.Append(AllStringsSE.qm + k + AllStringsSE.qm);
                if (to2 != i)
                {
                    sb.Append(AllStringsSE.comma);
                }
            }
        }
        else
        {
            for (int i = 0; i < to; i++)
            {
                string k = p[i].ToString();
                sb.Append("su.ToString(" + k + ")");
                if (to2 != i)
                {
                    sb.Append(AllStringsSE.comma);
                }
            }
        }

        sb.Append(AllStringsSE.rb);
    }

    public static Dictionary<string, string> ParseQs(NameValueCollection qs)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        foreach (var item in qs)
        {
            var key = item.ToString();
            var v = qs.Get(key);

            dict.Add(key, v);
        }

        return dict;
    }
}
