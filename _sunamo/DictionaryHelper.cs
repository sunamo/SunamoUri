namespace SunamoUri;
public class DictionaryHelper
{
    public static Dictionary<T, T> GetDictionaryByKeyValueInString<T>(List<T> p)
    {
        var methodName = Exc.CallingMethod();
        ThrowEx.IsOdd("p", p);

        Dictionary<T, T> result = new Dictionary<T, T>();
        for (int i = 0; i < p.Count; i++)
        {
            result.Add(p[i], p[++i]);
        }
        return result;
    }
}
