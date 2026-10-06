namespace SunamoUri._sunamo.SunamoCollectionsGeneric;

// This class must be here because SunamoValues cannot inherit from SunamoCollectionGeneric to avoid cycle detection.
internal class CAGConsts
{
    internal static List<T> ToList<T>(params T[] array)
    {
        return array.ToList();
    }
}
