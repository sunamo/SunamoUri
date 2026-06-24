namespace SunamoUri._sunamo.SunamoDictionary;

internal class DictionaryHelper
{
    internal static Dictionary<T, T> GetDictionaryByKeyValueInString<T>(List<T> list) where T : notnull
    {
        ThrowEx.HasOddNumberOfElements("list", list);

        var result = new Dictionary<T, T>();
        for (var i = 0; i < list.Count; i++) result.Add(list[i], list[++i]);
        return result;
    }
}
