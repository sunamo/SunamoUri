namespace SunamoUri;

public class QSHelper
{
    public static string? GetParameter(string uri, string parameterName)
    {
        var parts = uri.Split(new[] { "?", "&" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in parts)
        {
            var pair = item.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            if (pair[0] == parameterName) return pair[1];
        }

        return null;
    }

    public static string RemoveQs(string text)
    {
        var questionMarkIndex = text.IndexOf('?');
        if (questionMarkIndex != -1) return text.Substring(0, questionMarkIndex);
        return text;
    }

    public static string GetParameterSE(string uri, string parameterName)
    {
        parameterName = parameterName + "=";
        var startIndex = uri.IndexOf(parameterName);
        if (startIndex != -1)
        {
            var endIndex = uri.IndexOf("&", startIndex);
            startIndex = startIndex + parameterName.Length;
            if (endIndex != -1) return SHSubstring.Substring(uri, startIndex, endIndex) ?? "";

            return uri.Substring(startIndex);
        }

        return "";
    }

    public static string GetQS(string baseUrl, params string[] parameters)
    {
        var list = parameters.ToList();

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(baseUrl + "?");
        var pairCount = list.Count / 2 * 2;
        for (var i = 0; i < list.Count; i++)
        {
            if (i == pairCount) break;

            var key = list[i];
            var value = UH.UrlEncode(list[++i]);
            stringBuilder.Append(key + "=" + value + "&");
        }

        return stringBuilder.ToString().TrimEnd('&');
    }

    public static string GetQS(string baseUrl, Dictionary<string, string> parameters)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(baseUrl + "?");

        foreach (var item in parameters) stringBuilder.Append(item.Key + "=" + item.Value + "&");

        return stringBuilder.ToString().TrimEnd('&');
    }

    public static string? GetNormalizeQS(string text)
    {
        if (text.Length != 0)
        {
            if (text.Contains("contextkey=") || text.Contains("guid=") || text.Contains("SelectingPhotos="))
                return null;

            var parts = new List<string>(text.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries));
            parts.Sort();
            text = string.Join("&", parts.ToArray());
        }

        return text;
    }

    public static Dictionary<string, string> ParseQs(string queryString)
    {
        queryString = queryString.TrimStart('?');

        var parts = queryString.Split(new[] { "&", "=" }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        return DictionaryHelper.GetDictionaryByKeyValueInString(parts);
    }

    public static void GetArray(List<string> list, StringBuilder stringBuilder, bool isQuoted)
    {
        stringBuilder.Append("new Array(");
        var count = list.Count;
        if (list.Count == 1) count = 1;

        var lastIndex = count - 1;
        if (lastIndex == -1) lastIndex = 0;

        if (isQuoted)
            for (var i = 0; i < count; i++)
            {
                var element = list[i];
                stringBuilder.Append("\"" + element + "\"");
                if (lastIndex != i) stringBuilder.Append(",");
            }
        else
            for (var i = 0; i < count; i++)
            {
                var element = list[i];
                stringBuilder.Append("su.ToString(" + element + ")");
                if (lastIndex != i) stringBuilder.Append(",");
            }

        stringBuilder.Append(")");
    }

    public static Dictionary<string, string> ParseQs(NameValueCollection nameValueCollection)
    {
        var dictionary = new Dictionary<string, string>();

        foreach (var item in nameValueCollection)
        {
            var key = item?.ToString() ?? string.Empty;
            var value = nameValueCollection.Get(key) ?? string.Empty;

            dictionary.Add(key, value);
        }

        return dictionary;
    }
}
