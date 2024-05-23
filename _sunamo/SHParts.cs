namespace SunamoUri;

public class SHParts
{
    public static string RemoveAfterFirstChar(string name, char dot)
    {
        return RemoveAfterFirst(name, dot.ToString());
    }

    public static string TrimStart(string v, string s)
    {
        while (v.StartsWith(s))
        {
            v = v.Substring(s.Length);
        }

        return v;
    }

    public static string KeepAfterFirst(string searchQuery, string after, bool keepDeli = false)
    {
        var dx = searchQuery.IndexOf(after);
        if (dx != -1)
        {
            searchQuery = TrimStart(searchQuery.Substring(dx), after);
            if (keepDeli)
            {
                searchQuery = after + searchQuery;
            }
        }
        return searchQuery;
    }
    public static string RemoveAfterFirst(string t, string ch)
    {
        int dex = t.IndexOf(ch);
        if (dex == -1 || dex == t.Length - 1)
        {
            return t;
        }

        string vr = t.Remove(dex);
        return vr;
    }
}
