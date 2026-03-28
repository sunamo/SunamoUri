namespace SunamoUri._sunamo.SunamoDictionary;

/// <summary>
/// Provides helper methods for dictionary operations.
/// </summary>
internal class DictionaryHelper
{
    /// <summary>
    /// Creates a dictionary from a list where alternating elements are keys and values.
    /// </summary>
    /// <typeparam name="T">The type of keys and values.</typeparam>
    /// <param name="list">The list containing alternating key-value pairs.</param>
    /// <returns>A dictionary constructed from the key-value pairs.</returns>
    internal static Dictionary<T, T> GetDictionaryByKeyValueInString<T>(List<T> list) where T : notnull
    {
        ThrowEx.HasOddNumberOfElements("list", list);

        var result = new Dictionary<T, T>();
        for (var i = 0; i < list.Count; i++) result.Add(list[i], list[++i]);
        return result;
    }
}
