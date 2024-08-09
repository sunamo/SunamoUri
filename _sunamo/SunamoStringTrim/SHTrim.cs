namespace SunamoUri._sunamo.SunamoStringTrim;

internal class SHTrim
{
    internal static string TrimEnd(string name, string ext)
    {
        while (name.EndsWith(ext)) return name.Substring(0, name.Length - ext.Length);

        return name;
    }

    internal static string TrimStart(string v, string s)
    {
        while (v.StartsWith(s)) v = v.Substring(s.Length);

        return v;
    }
}