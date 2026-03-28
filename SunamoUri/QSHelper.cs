namespace SunamoUri;

/// <summary>
/// Query string helper methods for parsing and manipulating URL query strings.
/// </summary>
public class QSHelper
{
    /// <summary>
    /// Gets a parameter value from a URI query string.
    /// Returns null when not found. Use GetParameterSE for empty string fallback.
    /// </summary>
    /// <param name="uri">The URI containing the query string.</param>
    /// <param name="parameterName">The name of the parameter to find.</param>
    /// <returns>The parameter value, or null if not found.</returns>
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

    /// <summary>
    /// Removes the query string from a URI.
    /// </summary>
    /// <param name="text">The URI to process.</param>
    /// <returns>The URI without the query string.</returns>
    public static string RemoveQs(string text)
    {
        var questionMarkIndex = text.IndexOf('?');
        if (questionMarkIndex != -1) return text.Substring(0, questionMarkIndex);
        return text;
    }

    /// <summary>
    /// Gets a parameter value from a URI query string.
    /// Returns empty string when not found. Use GetParameter for null fallback.
    /// </summary>
    /// <param name="uri">The URI containing the query string.</param>
    /// <param name="parameterName">The name of the parameter to find.</param>
    /// <returns>The parameter value, or empty string if not found.</returns>
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

    /// <summary>
    /// Builds a query string URL from a base address and alternating key-value parameter pairs.
    /// All parameter values are automatically URL-encoded.
    /// </summary>
    /// <param name="baseUrl">The base URL without a trailing question mark.</param>
    /// <param name="parameters">Alternating key-value pairs for the query string.</param>
    /// <returns>The complete URL with query string.</returns>
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

    /// <summary>
    /// Builds a query string URL from a base address and a dictionary of parameters.
    /// </summary>
    /// <param name="baseUrl">The base URL without a trailing question mark.</param>
    /// <param name="parameters">Dictionary of parameter key-value pairs.</param>
    /// <returns>The complete URL with query string.</returns>
    public static string GetQS(string baseUrl, Dictionary<string, string> parameters)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(baseUrl + "?");

        foreach (var item in parameters) stringBuilder.Append(item.Key + "=" + item.Value + "&");

        return stringBuilder.ToString().TrimEnd('&');
    }

    /// <summary>
    /// Normalizes a query string by sorting parameters alphabetically.
    /// Returns null for tracking-related query strings (contextkey, guid, SelectingPhotos).
    /// </summary>
    /// <param name="text">The query string to normalize (without leading question mark).</param>
    /// <returns>The normalized query string, or null for tracking requests.</returns>
    public static string? GetNormalizeQS(string text)
    {
        if (text.Length != 0)
        {
            if (text.Contains("contextkey=") || text.Contains("guid=") || text.Contains("SelectingPhotos="))
                return null;

            var parts = new List<string>(text.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries));
            parts.Sort();
            text = string.Join('&', parts.ToArray());
        }

        return text;
    }

    /// <summary>
    /// Parses a query string into a dictionary of key-value pairs.
    /// Must receive just the query string without URI path. Use UH.GetQueryAsHttpRequest before calling.
    /// </summary>
    /// <param name="queryString">The query string to parse.</param>
    /// <returns>A dictionary of parameter names and values.</returns>
    public static Dictionary<string, string> ParseQs(string queryString)
    {
        queryString = queryString.TrimStart('?');

        var parts = queryString.Split(new[] { "&", "=" }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        return DictionaryHelper.GetDictionaryByKeyValueInString(parts);
    }

    /// <summary>
    /// Serializes a list of parameters into a JavaScript Array constructor call.
    /// </summary>
    /// <param name="list">The list of parameters to serialize.</param>
    /// <param name="stringBuilder">The StringBuilder to append to.</param>
    /// <param name="isQuoted">Whether to wrap values in double quotes.</param>
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

    /// <summary>
    /// Parses a NameValueCollection into a dictionary.
    /// </summary>
    /// <param name="nameValueCollection">The NameValueCollection to parse.</param>
    /// <returns>A dictionary of keys and values.</returns>
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
