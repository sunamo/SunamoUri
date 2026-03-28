namespace SunamoUri._sunamo.SunamoCollectionsGeneric;

/// <summary>
/// Collection constants and helper methods.
/// This class must be here because SunamoValues cannot inherit from SunamoCollectionGeneric to avoid cycle detection.
/// </summary>
internal class CAGConsts
{
    /// <summary>
    /// Converts a params array to a List.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    /// <param name="array">The elements to convert.</param>
    /// <returns>A list containing the elements.</returns>
    internal static List<T> ToList<T>(params T[] array)
    {
        return array.ToList();
    }
}
