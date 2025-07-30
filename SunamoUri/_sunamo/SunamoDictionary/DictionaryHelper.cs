namespace SunamoUri._sunamo.SunamoDictionary;
internal class DictionaryHelper
{
    internal static Dictionary<T, T> GetDictionaryByKeyValueInString<T>(List<T> p)
    {
        var methodName = Exceptions.CallingMethod();
        ThrowEx.HasOddNumberOfElements("p", p);

        var result = new Dictionary<T, T>();
        for (var i = 0; i < p.Count; i++) result.Add(p[i], p[++i]);
        return result;
    }
}