namespace SunamoUri;

/// <summary>
///     Summary description for QSHelper
/// </summary>
public class QSHelper
{
    /// <summary>
    ///     GetParameter = return null when not found
    ///     GetParameterSE = return string.Empty when not found
    /// </summary>
    public static string GetParameter(string uri, string nameParam)
    {
        var main = uri.Split(new[] { "?", "&" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var var in main)
        {
            var value = var.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            if (value[0] == nameParam) return value[1];
        }

        return null;
    }

    public static string RemoveQs(string value)
    {
        var xValue = value.IndexOf('?');
        if (xValue != -1) return value.Substring(0, xValue);
        return value;
        //
    }

    /// <summary>
    ///     get value of A2 parametr in A1
    ///     GetParameter = return null when not found
    ///     GetParameterSE = return string.Empty when not found
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="nameParam"></param>
    public static string GetParameterSE(string uri, string nameParam)
    {
        nameParam = nameParam + "=";
        var dexPocatek = uri.IndexOf(nameParam);
        if (dexPocatek != -1)
        {
            var dexKonec = uri.IndexOf("&", dexPocatek);
            dexPocatek = dexPocatek + nameParam.Length;
            if (dexKonec != -1) return SHSubstring.Substring(uri, dexPocatek, dexKonec);

            return uri.Substring(dexPocatek);
        }

        return "";
    }

    /// <summary>
    ///     A1 je adresa bez konzového otazníku
    ///     Všechny parametry automaticky zakóduje metodou UH.UrlEncode
    /// </summary>
    /// <param name="adresa"></param>
    /// <param name="p"></param>
    public static string GetQS(string adresa, params string[] p2)
    {
        //var parameter = CA.TwoDimensionParamsIntoOne(p2);
        var parameter = p2.ToList();

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(adresa + "?");
        var to = parameter.Count / 2 * 2;
        for (var i = 0; i < parameter.Count; i++)
        {
            if (i == to) break;

            var k = parameter[i];
            var value = UH.UrlEncode(parameter[++i]);
            stringBuilder.Append(k + "=" + value + "&");
        }

        return stringBuilder.ToString().TrimEnd('&');
    }

    public static string GetQS(string adresa, Dictionary<string, string> p2)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(adresa + "?");

        foreach (var item in p2) stringBuilder.Append(item.Key + "=" + item.Value + "&");

        return stringBuilder.ToString().TrimEnd('&');
    }

    /// <summary>
    ///     Do A1 se zadává Request.Url.Query.Substring(1) neboli třeba pid=1&amp;aid=10
    /// </summary>
    /// <param name="args"></param>
    public static string GetNormalizeQS(string args)
    {
        if (args.Length != 0)
        {
            if (args.Contains("contextkey=") || args.Contains("guid=") || args.Contains("SelectingPhotos="))
                // Pouze uploaduji fotky pomocí AjaxControlToolkit, ¨přece nebudu každé odeslání fotky ukládat do DB
                return null;

            //args = args.Substring(1);
            var splited = new List<string>(args.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries));
            splited.Sort();
            args = string.Join('&', splited.ToArray());
        }

        return args;
    }

    /// <summary>
    ///     Must get just qs without uri => use UH.GetQueryAsHttpRequest before
    /// </summary>
    /// <param name="qs"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ParseQs(string qs)
    {
        var dictionary = new Dictionary<string, string>();

        qs = qs.TrimStart('?');

        var parts = qs.Split(new[] { "&", "=" }, StringSplitOptions.RemoveEmptyEntries)
            .ToList(); // SHSplit.Split(qs, );

        return DictionaryHelper.GetDictionaryByKeyValueInString(parts);
    }

    public static void GetArray(List<string> parameter, StringBuilder stringBuilder, bool uvo)
    {
        stringBuilder.Append("new Array(");
        //int to = (parameter.Length / 2) * 2;
        var to = parameter.Count;
        if (parameter.Count == 1) to = 1;

        var to2 = to - 1;
        if (to2 == -1) to2 = 0;

        if (uvo)
            for (var i = 0; i < to; i++)
            {
                var k = parameter[i];
                stringBuilder.Append("\"" + k + "\"");
                if (to2 != i) stringBuilder.Append(",");
            }
        else
            for (var i = 0; i < to; i++)
            {
                var k = parameter[i];
                stringBuilder.Append("su.ToString(" + k + ")");
                if (to2 != i) stringBuilder.Append(",");
            }

        stringBuilder.Append(")");
    }

    public static Dictionary<string, string> ParseQs(NameValueCollection qs)
    {
        var dict = new Dictionary<string, string>();

        foreach (var item in qs)
        {
            var key = item.ToString();
            var value = qs.Get(key);

            dict.Add(key, value);
        }

        return dict;
    }
}